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
    public class ProjectController : Controller
    {
        #region properties

        protected UserManager<ApplicationUser> UserManager { get; set; }
        private readonly ProjectService _projectService;
        private readonly ClientService _clientService;

        #endregion

        public ProjectController()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            _projectService = new ProjectService();
            _clientService = new ClientService();
        }

        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetList(DataTablesParam param, string sortDir, string sortCol, DateTime? startDate, DateTime? endDate)
        {
            var projectsVm = new List<ProjectViewModel>();
            int pageNo = 1;
            var user = UserManager.FindById(User.Identity.GetUserId());

            if (param.iDisplayStart >= param.iDisplayLength)
                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;

            int totalCount = 0;

            if (param.sSearch != null)
            {
                sortCol = sortCol == "CreatedByName" ? "CreatedBy" : sortCol;
                projectsVm = _projectService.GetAllAfterSearch(param, startDate, endDate)
                .OrderBy(x => x.Id).OrderBy(sortCol + " " + sortDir).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                .Select(x => new ProjectViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ClientName = x.Client.Name,
                    CreatedByName = x.AspNetUser?.UserDetails?.Max(s => (string.Format("{0} {1}", s.FirstName, s.LastName))),
                    CreatedOn = x.CreatedOn,
                    IsCurrentUser = user.Id == x.CreatedBy ? true : false,
                    IsUnderDeleteTime = ((DateTime.Now - x.CreatedOn).TotalDays < 7) ? true : false
                }).ToList();
                totalCount = projectsVm.Count();
            }
            else
            {
                sortCol = sortCol == "CreatedByName" ? "CreatedBy" : sortCol;
                totalCount = _projectService.GetAll(startDate, endDate).Count();
                projectsVm = _projectService.GetAll(startDate, endDate).OrderBy(x => x.Id).OrderBy(sortCol + " " + sortDir)
                    .Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength)
                     .Select(x => new ProjectViewModel
                     {
                         Id = x.Id,
                         Name = x.Name,
                         ClientName = x.Client.Name,
                         CreatedByName = x.AspNetUser?.UserDetails?.Max(s => (string.Format("{0} {1}", s.FirstName, s.LastName))),
                         CreatedOn = x.CreatedOn,
                         IsCurrentUser = user.Id == x.CreatedBy ? true : false,
                         IsUnderDeleteTime = ((DateTime.Now - x.CreatedOn).TotalDays < 7) ? true : false
                     })
                .ToList();
            }

            return Json(new
            {
                aaData = projectsVm,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddProject(int id, string viewType)
        {
            ProjectViewModel vm = id > 0 ? _projectService.GetByIDVM(id) : new ProjectViewModel();
            if (viewType == "Display")
            {
                vm.ViewType = "Display";
            }
            else
            {
                var clients = _clientService.GetAll();
                vm.Clients = clients
                    .Select(x => new SelectListItem { Text = string.Format("{0}", x.Name), Value = x.Id.ToString() })
                    .OrderBy(x => x.Text)
                    .ToList();
            }

            return Json(new { Success = true, Html = this.RenderPartialViewToString("_AddEditProject", vm) }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Insert update Project
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertUpdate(ProjectViewModel vm)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (vm.Id < 1)
            {
                vm.CreatedBy = user.Id;
            }
            vm.IsActive = true;
            _projectService.InsertUpdate(vm);
            if (vm.Id > 0)
            {
                return Json(new { Message = "Project updated successfully!", Success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = "Project inserted successfully!", Success = true }, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// delete project
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Delete(int Id)
        {
            ProjectViewModel projectVM = new ProjectViewModel();
            projectVM.Id = Id;
            projectVM.IsActive = false;
            if (User.IsInRole("Admin") || User.IsInRole("HR"))
            {
                _projectService.Delete(projectVM);
                return Json(new { Message = "Project deleted successfully!", Success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Check billing first
                //var billingCount = _billingService.GetBillingCount(ControllerTypeEnum.ControllerType.Client, Id);
                //if (billingCount <= 0)
                //{
                _projectService.Delete(projectVM);
                return Json(new { Message = "Project deleted successfully!", Success = true }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return Json(new { Message = "Sorry! You can't delete this record!", Success = false }, JsonRequestBehavior.AllowGet);
                //}
            }
        }
    }
}