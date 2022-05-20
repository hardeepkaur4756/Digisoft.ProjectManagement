using Digisoft.ProjectManagement.Helper;
using Digisoft.ProjectManagement.Models;
using Digisoft.ProjectManagement.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace Digisoft.ProjectManagement.Controllers
{
    [Authorize]
    public class WorkingController : Controller
    {
        #region properties

        protected UserManager<ApplicationUser> UserManager { get; set; }
        private readonly WorkingService _workingService;
        private readonly ProjectService _projectService;
        private readonly UserService _userService;
        #endregion

        public WorkingController()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            _workingService = new WorkingService();
            _projectService = new ProjectService();
            _userService = new UserService();
        }

        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetList(DataTablesParam param, string sortDir, string sortCol, DateTime? startDate, DateTime? endDate, string projectId, string userId)
        {
            var workingsVM = new List<WorkingViewModel>();
            int pageNo = 1;
            int project = int.Parse(projectId);
            userId = string.IsNullOrEmpty(userId) ? null : userId;
            var user = UserManager.FindById(User.Identity.GetUserId());

            if (param.iDisplayStart >= param.iDisplayLength)
                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;

            int totalCount = 0;
            string totalHoursWorked = "";
            var totalHoursBilled = "";
            if (param.sSearch != null)
            {
                sortCol = sortCol == "CreatedByName" ? "CreatedBy" : sortCol;
                workingsVM = _workingService.GetAllAfterSearch(param, startDate, endDate, project, userId)
                .OrderBy(x => x.Id).OrderBy(sortCol + " " + sortDir).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                .Select(x => new WorkingViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    ProjectName = x.Project.Name,
                    HoursBilled = x.HoursBilled,
                    HoursWorked = x.HoursWorked,
                    DateWorked = x.DateWorked,
                    CreatedByName = x.AspNetUser?.UserDetails?.Max(s => (string.Format("{0} {1}", s.FirstName, s.LastName))),
                    CreatedDate = x.CreatedDate,
                    IsCurrentUser = user.Id == x.CreatedBy ? true : false,
                    IsUnderDeleteTime = ((DateTime.Now - x.CreatedDate).TotalDays < 7) ? true : false
                }).ToList();
                totalCount = workingsVM.Count();
                totalHoursWorked = workingsVM.Sum(x=> x.HoursWorked).ToString();
                totalHoursBilled = workingsVM.Sum(x => x.HoursBilled).ToString();
            }
            else
            {
                sortCol = sortCol == "CreatedByName" ? "CreatedBy" : sortCol;
                totalCount = _workingService.GetAll(startDate, endDate, project, userId).Count();
                workingsVM = _workingService.GetAll(startDate, endDate, project, userId).OrderBy(x => x.Id).OrderBy(sortCol + " " + sortDir)
                    .Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                     .Select(x => new WorkingViewModel
                     {
                         Id = x.Id,
                         Description = x.Description,
                         ProjectName = x.Project.Name,
                         HoursBilled = x.HoursBilled,
                         HoursWorked = x.HoursWorked,
                         DateWorked = x.DateWorked,
                         CreatedByName = x.AspNetUser?.UserDetails?.Max(s => (string.Format("{0} {1}", s.FirstName, s.LastName))),
                         CreatedDate = x.CreatedDate,
                         IsCurrentUser = user.Id == x.CreatedBy ? true : false,
                         IsUnderDeleteTime = ((DateTime.Now - x.CreatedDate).TotalDays < 7) ? true : false
                     })
                .ToList();
                totalHoursWorked = workingsVM.Sum(x => x.HoursWorked).ToString();
                totalHoursBilled = workingsVM.Sum(x => x.HoursBilled).ToString();
            }
            return Json(new
            {
                aaData = workingsVM,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount,
                totalHoursWorked= totalHoursWorked,
                totalHoursBilled = totalHoursBilled,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddWorking(int id, string viewType)
        {
            WorkingViewModel vm = id > 0 ? _workingService.GetByIDVM(id) : new WorkingViewModel();

            if (viewType == "Display")
            {
                vm.ViewType = "Display";
            }
            else
            {
                var projects = _projectService.GetAll();
                vm.Projects = projects
                    .Select(x => new SelectListItem { Text = string.Format("{0}", x.Name), Value = x.Id.ToString() })
                    .OrderBy(x => x.Text)
                    .ToList();
            }

            return Json(new { Success = true, Html = this.RenderPartialViewToString("_AddEditWorking", vm) }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Insert update Project
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertUpdate(WorkingViewModel vm)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (vm.Id < 1)
            {
                vm.CreatedBy = user.Id;
            }
            vm.IsActive = true;
            _workingService.InsertUpdate(vm);
            if (vm.Id > 0)
            {
                return Json(new { Message = "Working updated successfully!", Success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = "Working inserted successfully!", Success = true }, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// delete project
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Delete(int Id)
        {
            WorkingViewModel workingVM = new WorkingViewModel();
            workingVM.Id = Id;
            workingVM.IsActive = false;
            if (User.IsInRole("Admin") || User.IsInRole("HR"))
            {
                _workingService.Delete(workingVM);
                return Json(new { Message = "Working deleted successfully!", Success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Check billing first
                //var billingCount = _billingService.GetBillingCount(ControllerTypeEnum.ControllerType.Client, Id);
                //if (billingCount <= 0)
                //{
                _workingService.Delete(workingVM);
                return Json(new { Message = "Working deleted successfully!", Success = true }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return Json(new { Message = "Sorry! You can't delete this record!", Success = false }, JsonRequestBehavior.AllowGet);
                //}
            }
        }
        public ActionResult GetProject()
        {
            WorkingViewModel vm = new WorkingViewModel();
            var projects = _projectService.GetAllForFilter();
            vm.Projects = projects
                   .Select(x => new SelectListItem { Text = string.Format("{0}", x.Name), Value = x.Id.ToString() })
                   .OrderBy(x => x.Text)
                   .ToList();
            return Json(vm.Projects, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUser()
        {
            UserViewModel vm = new UserViewModel();
            var users = _userService.GetAllUser();
            vm.Users = users
                   .Select(x => new SelectListItem { Text = string.Format("{0}", x.FirstName + " " + x.LastName), Value = x.UserId.ToString() })
                   .OrderBy(x => x.Text)
                   .ToList();
            return Json(vm.Users, JsonRequestBehavior.AllowGet);
        }
    }
}