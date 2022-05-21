using Digisoft.ProjectManagement.Helper;
using Digisoft.ProjectManagement.Models;
using Digisoft.ProjectManagement.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Digisoft.ProjectManagement.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {

        #region properties

        protected UserManager<ApplicationUser> UserManager { get; set; }
        private readonly UserAttendanceService _userAttendanceService;
        private readonly UserService _userService;
        private ProjectManagementEntities _context;
        #endregion
        public ActionResult Index()
        {
            UserAttendanceViewModel attendance = new UserAttendanceViewModel();
            var users = _userService.GetAllUser();
            attendance.Users = users
                   .Select(x => new SelectListItem { Text = string.Format("{0}", x.FirstName + " " + x.LastName), Value = x.UserId.ToString() })
                   .OrderBy(x => x.Text)
                   .ToList();
            return View(attendance);
        }
        public AttendanceController()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            _userAttendanceService = new UserAttendanceService();
            _userService = new UserService();
            _context = new ProjectManagementEntities();
        }
        public ActionResult GetList(DataTablesParam param, string sortDir, string sortCol, DateTime? date, string userId)
        {
            var attendVM = new List<UserAttendanceViewModel>();
            int pageNo = 1;
            var user = UserManager.FindById(User.Identity.GetUserId());

            if (param.iDisplayStart >= param.iDisplayLength)
                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;

            int totalCount = 0;

            if (param.sSearch != null)
            {
                sortCol = sortCol == "CreatedByName" ? "CreatedBy" : sortCol;
                attendVM = _userAttendanceService.GetAllAfterSearch(param, date, userId)
                .OrderBy(x => x.Id).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                .Select(x => new UserAttendanceViewModel
                {
                    Id = x.Id,
                    AttendanceStatus = "",
                    InTime = x.InTime,
                    OutTime = x.OutTime,
                    Date = x.Date,
                    OnLeave = x.OnLeave,
                    LeaveApprovedBy = x.LeaveApprovedBy,
                    UserName = x.LeaveApprovedBy,
                    Comment = x.Comment,
                    CreatedByName = x.AspNetUser?.UserDetails?.Max(s => (string.Format("{0} {1}", s.FirstName, s.LastName))),
                    CreatedDate = x.CreatedDate,
                    IsCurrentUser = user.Id == x.CreatedBy ? true : false
                }).ToList();
                totalCount = attendVM.Count();
            }
            else
            {
                sortCol = sortCol == "CreatedByName" ? "CreatedBy" : sortCol;
                totalCount = _userAttendanceService.GetAll(date).Count();
                attendVM = _userAttendanceService.GetAll(date).OrderBy(x => x.Id)
                    .Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                     .Select(x => new UserAttendanceViewModel
                     {
                         Id = x.Id,
                         AttendanceStatus = "",
                         InTime = x.InTime,
                         OutTime = x.OutTime,
                         Date = x.Date,
                         OnLeave = x.OnLeave,
                         LeaveApprovedBy = x.LeaveApprovedBy,
                         UserName = x.LeaveApprovedBy,
                         Comment = x.Comment,
                         CreatedByName = x.AspNetUser?.UserDetails?.Max(s => (string.Format("{0} {1}", s.FirstName, s.LastName))),
                         CreatedDate = x.CreatedDate,
                         IsCurrentUser = user.Id == x.CreatedBy ? true : false
                     })
                .ToList();
            }

            return Json(new
            {
                aaData = attendVM,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AddAttendance(string id, string viewType)
        {
            UserAttendanceViewModel vm = new UserAttendanceViewModel();
            if (viewType == "Display")
            {
                vm.ViewType = "Display";
            }
            else
            {
                var date = DateTime.Now.Date;
                //List<UserDetail> lst = new List<UserDetail>();
                //var users = (from a in _context.UserAttendances
                //             join c in _context.UserDetails on a.UserId equals c.UserId
                //             where c.UserId != a.UserId && a.Date != date
                //             select new
                //             {
                //                 UserId = c.UserId,
                //                 Name = c.FirstName + ' ' + c.LastName == null ? "" : c.LastName,
                //             }).ToList();
                //foreach(var item in users)
                //{
                //    UserDetail model = new UserDetail();
                //    model.UserId = item.UserId;
                //    model.FirstName = item.Name;
                //    lst.Add(model);
                //}
                //vm.UserDetails = lst;
                var Users = _userService.GetAllUser().ToList();
                vm.UserDetails = Users;
            }
            return Json(new { Success = true, Html = this.RenderPartialViewToString("_AddEditAttendance", vm) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult MarkAttendance(UserAttendanceViewModel model)
        {
            try
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                if (model.Id < 1)
                {
                    model.CreatedBy = user.Id;
                }
                if (model.AttendanceStatus == "Present")
                {
                    model.InTime = new TimeSpan(9, 45, 00);
                    model.OutTime = new TimeSpan(7, 00, 00);
                    model.Date = DateTime.Now.Date;
                    _userAttendanceService.InsertUpdate(model);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(new { Message = "Attendance marked successfully!", Success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}