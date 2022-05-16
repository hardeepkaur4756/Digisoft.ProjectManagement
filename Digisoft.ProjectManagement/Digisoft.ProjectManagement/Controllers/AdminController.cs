using Digisoft.ProjectManagement.Helper;
using Digisoft.ProjectManagement.Models;
using Digisoft.ProjectManagement.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Digisoft.ProjectManagement.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        protected UserManager<ApplicationUser> UserManager { get; set; }
        private readonly UserService _userService;
        private ProjectManagementEntities _context;
        public AdminController()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            _context = new ProjectManagementEntities();
            _userService = new UserService();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }
        [Authorize(Roles = "Admin,HR")]
        public ActionResult Index()
        {
            try
            {
                UserViewModel user = new UserViewModel();

                var result = (from a in _context.AspNetUsers
                              join c in _context.UserDetails on a.Id equals c.UserId
                              select new
                              {
                                  UserId = a.Id,
                                  FirstName = c.FirstName,
                                  LastName = c.LastName,
                                  Email = a.Email,
                                  IsActive = c.Exclude
                              }).ToList();
                List<UserViewModel> lst = new List<UserViewModel>();
                foreach (var item in result)
                {
                    UserViewModel model = new UserViewModel();
                    model.UserId = item.UserId;
                    model.Email = item.Email;
                    model.FirstName = item.FirstName;
                    model.LastName = item.LastName;
                    model.Exclude = Convert.ToBoolean(item.IsActive);
                    lst.Add(model);
                }
                if (User.IsInRole("Admin") || User.IsInRole("HR"))
                {
                    return View(lst);
                }
                else
                {
                    return View("User");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult AddUser(string id, string viewType)
        {
            try
            {
                string role = "";
                if (id != "0")
                {
                    role = UserManager.GetRoles(id).FirstOrDefault();
                }
                var vm = new UserViewModel();
                vm.ViewType = viewType;
                vm.Users = _context.AspNetRoles.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList();
                vm.States = _context.States.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Value).ToList();
                vm.Countries = _context.Countries.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Value).ToList();
                vm.Departments = _context.Departments.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Value).ToList();
                if (id != null && id != "" && id != "0" && viewType == "")
                {
                    var result = (from a in _context.AspNetUsers
                                  join c in _context.UserDetails on a.Id equals c.UserId
                                  select new
                                  {
                                      UserId = a.Id,
                                      FirstName = c.FirstName,
                                      LastName = c.LastName,
                                      Email = a.Email,
                                      IsActive = c.Exclude
                                  }).Where(x => x.UserId == id).FirstOrDefault();

                    vm.UserId = result.UserId;
                    vm.Email = result.Email;
                    vm.FirstName = result.FirstName;
                    vm.LastName = result.LastName;
                    vm.Exclude = Convert.ToBoolean(result.IsActive);
                }
                else if (viewType == "ChangePassword")
                {
                    vm.ViewType = viewType;
                    vm.UserId = id;
                    vm.FirstName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.FirstName).FirstOrDefault();
                }
                else if (viewType == "View")
                {
                    vm = id != null ? _userService.GetByIDVM(id) : new UserViewModel();
                    vm.RoleName = role;
                    vm.ViewType = viewType;
                }
                else if (viewType == "UploadDocument")
                {
                    vm.ViewType = viewType;
                    vm.UserId = id;
                    vm.FirstName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.FirstName).FirstOrDefault();
                }
                else if (viewType == "ViewDocument")
                {
                    List<UserDocument> lst = new List<UserDocument>();
                    vm.UserId = id;
                    vm.FirstName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.FirstName).FirstOrDefault();
                    vm.ViewType = viewType;
                    var Documents = (from a in _context.UserDocuments
                                     where a.UserId == id
                                     select new
                                     {
                                         UserId = a.UserId,
                                         Id = a.Id,
                                         Type = a.Type,
                                         Name = "/Documents/" + a.Name,
                                     }).ToList();
                    foreach (var item in Documents)
                    {
                        UserDocument model = new UserDocument();
                        model.UserId = item.Name;
                        model.Name = item.Name;
                        model.Id = item.Id;
                        model.Type = item.Type;
                        lst.Add(model);
                    }
                    vm.Documents = lst;
                }
                else if (id != "0" && viewType == "Display")
                {
                    vm = id != null ? _userService.GetByIDVM(id) : new UserViewModel();
                    vm.RoleId = _context.AspNetRoles.Where(x => x.Name == role).Select(x => x.Id).FirstOrDefault();
                    vm.ViewType = viewType;
                    vm.Users = _context.AspNetRoles.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList();
                    vm.States = _context.States.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Value).ToList();
                    vm.Countries = _context.Countries.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Value).ToList();
                    vm.Departments = _context.Departments.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Value).ToList();
                }
                return Json(new { Success = true, UserName = vm.FirstName, Html = this.RenderPartialViewToString("_AddEditUser", vm) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddUser(UserViewModel u)
        {

            try
            {
                if (u.UserId == null)
                {
                    var Exist = _context.AspNetUsers.Where(x => x.Email == u.Email).FirstOrDefault();
                    if (Exist == null)
                    {
                        var user = new ApplicationUser { UserName = u.Email, Email = u.Email };
                        var result = await UserManager.CreateAsync(user, u.Password);
                        if (result.Succeeded)
                        {
                            result = UserManager.AddToRole(user.Id, u.RoleId);
                        }
                        u.UserId = user.Id;
                        _userService.Insert(u);
                        return Json(new { Message = "User added successfully!", Success = true });
                    }
                    else
                    {
                        return Json(new { Message = "Email already exist", Success = false });
                    }
                }
                else
                {
                    UserDetail m = new UserDetail();
                    using (var ctx = new ProjectManagementEntities())
                    {
                        m = ctx.UserDetails.Where(s => s.UserId == u.UserId).FirstOrDefault();
                    }
                    if (m != null)
                    {
                        m.Exclude = Convert.ToBoolean(u.Exclude);
                        m.FirstName = u.FirstName;
                        m.LastName = u.LastName;
                        m.UserId = u.UserId;
                        string OldRoleName = UserManager.GetRoles(u.UserId).FirstOrDefault();
                        if (OldRoleName != u.RoleId)
                        {
                            UserManager.RemoveFromRole(m.UserId, u.RoleId);
                            var r = UserManager.AddToRole(m.UserId, u.RoleId);
                        }
                        m.ExpWhenJoined = u.ExpWhenJoined;
                        m.PhoneNumber = u.PhoneNumber;
                        m.AlternatePhoneNumber = u.AlternatePhoneNumber;
                        m.PersonalEmailAddress = u.PersonalEmailAddress;
                        m.DepartmentId = u.DepartmentId;
                        m.DOB = u.DOB;
                        m.DateofJoining = u.DateofJoining;
                        m.DateofRelieving = u.DateofRelieving;
                        m.CurrentCity = u.CurrentCity;
                        m.PermanentCity = u.PermanentCity;
                        m.CurrentStateId = u.CurrentStateId;
                        m.PermanentStateId = u.PermanentStateId;
                        m.PermanentCountryId = u.PermanentCountryId;
                        m.PermanentAddress = u.PermanentAddress;
                        m.CurrentCountryId = u.CurrentCountryId;
                        m.CurrentAddress = u.CurrentAddress;
                        m.AadharNumber = u.AadharNumber;
                        m.PanNumber = u.PanNumber;
                        m.Skills = u.Skills;
                        m.EmergencyContactName = u.EmergencyContactName;
                        m.EmergencyContactNumber = u.EmergencyContactNumber;
                        m.EmergencyContactRelation = u.EmergencyContactRelation;
                        m.Salary = u.Salary;
                    }
                    using (var dbCtx = new ProjectManagementEntities())
                    {
                        dbCtx.Entry(m).State = EntityState.Modified;
                        dbCtx.SaveChanges();
                    }
                    return Json(new { Message = "User updated successfully!", Success = true }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> ChangePasswordAsync(UserViewModel model)
        {
            try
            {
                AspNetUser m = new AspNetUser();
                using (var ctx = new ProjectManagementEntities())
                {
                    m = ctx.AspNetUsers.Where(s => s.Id == model.UserId).FirstOrDefault();
                }

                ApplicationUser user = await UserManager.FindByIdAsync(model.UserId);
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { Message = "Password changed successfully!", Success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Message = result.Errors, Success = false }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult UploadDocument(UserViewModel userViewModel)
        {
            try
            {
                var Success = false;
                var Message = "";
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            UserDocument ud = new UserDocument()
                            {
                                UserId = userViewModel.UserId,
                                Name = fileName,
                                Type = userViewModel.Type
                            };
                            var path = Path.Combine(Server.MapPath("~/Documents"), ud.Name);
                            file.SaveAs(path);
                            using (var dbCtx = new ProjectManagementEntities())
                            {


                                dbCtx.Entry(ud).State = EntityState.Added;
                                dbCtx.SaveChanges();
                                Success = true;
                                Message = "Document Uploaded Successfully";
                            }
                        }
                    }
                }
                return Json(new { Message = Message, Success = Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ActionResult DeleteDocument(int Id)
        {
            var d = _context.UserDocuments.SingleOrDefault(x => x.Id == Id); //returns a single item.
            if (d != null)
            {
                _context.UserDocuments.Remove(d);
                _context.SaveChanges();
            }
            return Json(new { Message = "Document deleted successfully!", Success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}