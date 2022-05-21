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
                                  IsActive = c.Exclude,
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
                    model.RoleName = UserManager.GetRoles(item.UserId).FirstOrDefault();
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
                string UserName = "";
                if (id != "0" && viewType != "AddEducation" && viewType != "EditEducation" && viewType != "AddIncrement" && viewType != "EditIncrement")
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
                    vm.LastName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.LastName).FirstOrDefault();
                }
                else if (viewType == "AddEducation")
                {
                    vm.ViewType = viewType;
                    vm.UserId = id;
                    vm.FirstName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.FirstName).FirstOrDefault();
                    vm.LastName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.LastName).FirstOrDefault();
                    vm.Courses = _context.Courses.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Value).ToList();
                }
                else if (viewType == "EditEducation")
                {
                    vm.ViewType = viewType;
                    vm.Id = int.Parse(id);
                    var result = (from a in _context.UserEducations
                                  where a.Id == vm.Id
                                  select new
                                  {
                                      UserId = a.UserId,
                                      Id = a.Id,
                                      CourseId = a.CourseId,
                                      Percentage = a.Percentage,
                                      YearPassed = a.YearPassed,
                                      Comment = a.Comment
                                  }).FirstOrDefault();

                    vm.UserId = result.UserId;
                    vm.Id = result.Id;
                    vm.CourseId = result.CourseId;
                    vm.Percentage = result.Percentage;
                    vm.YearPassed = result.YearPassed;
                    vm.Comment = result.Comment;
                    vm.FirstName = _context.UserDetails.Where(x => x.UserId == result.UserId).Select(x => x.FirstName).FirstOrDefault();
                    vm.LastName = _context.UserDetails.Where(x => x.UserId == result.UserId).Select(x => x.LastName).FirstOrDefault();
                    vm.Courses = _context.Courses.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).OrderBy(x => x.Value).ToList();
                }
                else if (viewType == "AddIncrement")
                {
                    vm.ViewType = viewType;
                    vm.UserId = id;
                    vm.FirstName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.FirstName).FirstOrDefault();
                    vm.LastName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.LastName).FirstOrDefault();
                }
                else if (viewType == "EditIncrement")
                {
                    vm.ViewType = viewType;
                    vm.Id = int.Parse(id);
                    var result = (from a in _context.UserIncrements
                                  where a.Id == vm.Id
                                  select new
                                  {
                                      UserId = a.UserId,
                                      Id = a.Id,
                                      DateOfIncrement = a.DateOfIncrement,
                                      Salary = a.Salary
                                  }).FirstOrDefault();

                    vm.UserId = result.UserId;
                    vm.Id = result.Id;
                    vm.DateOfIncrement = result.DateOfIncrement;
                    vm.Salary = result.Salary;
                    vm.FirstName = _context.UserDetails.Where(x => x.UserId == result.UserId).Select(x => x.FirstName).FirstOrDefault();
                    vm.LastName = _context.UserDetails.Where(x => x.UserId == result.UserId).Select(x => x.LastName).FirstOrDefault();
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
                    vm.LastName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.LastName).FirstOrDefault();
                }
                else if (viewType == "ViewDocument")
                {
                    List<UserDocument> lst = new List<UserDocument>();
                    vm.UserId = id;
                    vm.FirstName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.FirstName).FirstOrDefault();
                    vm.LastName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.LastName).FirstOrDefault();

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
                else if (viewType == "ViewEducation")
                {
                    List<UserEducationViewModel> lst = new List<UserEducationViewModel>();
                    vm.UserId = id;
                    vm.FirstName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.FirstName).FirstOrDefault();
                    vm.LastName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.LastName).FirstOrDefault();

                    vm.ViewType = viewType;
                    var Educations = (from a in _context.UserEducations
                                      where a.UserId == id
                                      select new
                                      {
                                          UserId = a.UserId,
                                          Id = a.Id,
                                          Percentage = a.Percentage,
                                          YearPassed = a.YearPassed,
                                          Comment = a.Comment,
                                          CourseName = a.Course.Name,
                                      }).ToList();
                    foreach (var item in Educations)
                    {
                        UserEducationViewModel model = new UserEducationViewModel();
                        model.UserId = item.UserId;
                        model.Id = item.Id;
                        model.Percentage = item.Percentage;
                        model.YearPassed = item.YearPassed;
                        model.Comment = item.Comment;
                        model.CourseName = item.CourseName;
                        string isU = _context.UserDocuments.Where(x => x.UserId == item.UserId && x.Type == model.CourseName).Select(x => x.Id).FirstOrDefault().ToString();
                        model.IsDocumentUploaded = isU != "0" ? true : false;
                        lst.Add(model);
                    }
                    vm.Educations = lst;
                }
                else if (viewType == "ViewIncrement")
                {
                    List<UserIncrementViewModel> lst = new List<UserIncrementViewModel>();
                    vm.UserId = id;
                    vm.FirstName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.FirstName).FirstOrDefault();
                    vm.LastName = _context.UserDetails.Where(x => x.UserId == id).Select(x => x.LastName).FirstOrDefault();

                    vm.ViewType = viewType;
                    var Increments = (from a in _context.UserIncrements
                                      where a.UserId == id
                                      select new
                                      {
                                          UserId = a.UserId,
                                          Id = a.Id,
                                          DateOfIncrement = a.DateOfIncrement,
                                          CurrentSalary = a.Salary,
                                      }).ToList();
                    int i = 1;
                    foreach (var item in Increments)
                    {
                        UserIncrementViewModel model = new UserIncrementViewModel();
                        model.UserId = item.UserId;
                        model.Id = item.Id;
                        model.DateOfIncrement = item.DateOfIncrement == null ? null : item.DateOfIncrement.Value.ToString("dd-MM-yyyy");
                        model.CurrentSalary = item.CurrentSalary;
                        if (i == 1)
                        {
                            model.PreviousSalary = _context.UserDetails.Where(x => x.UserId == model.UserId).Select(x => x.Salary).FirstOrDefault();
                        }
                        else
                        {
                            model.PreviousSalary = Increments.TakeWhile(x => x.Id != item.Id).Select(x => x.CurrentSalary).LastOrDefault();
                        }
                        model.Amount = model.CurrentSalary - model.PreviousSalary;
                        model.Percentage = Math.Round(Convert.ToDouble(model.Amount / model.PreviousSalary * 100), 2);
                        lst.Add(model);
                        i++;
                    }
                    vm.Increments = lst;
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
                UserName = vm.FirstName + " " + vm.LastName;
                return Json(new { Success = true, UserName = UserName, Html = this.RenderPartialViewToString("_AddEditUser", vm) }, JsonRequestBehavior.AllowGet);
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
                        try
                        {
                            u.UserId = user.Id;
                            _userService.Insert(u);
                        }
                        catch (Exception ex)
                        {
                            var us = _context.AspNetUsers.Where(x => x.Id == u.UserId).FirstOrDefault();
                            UserManager.RemoveFromRole(u.UserId, u.RoleId);
                            _context.AspNetUsers.Remove(us);
                            _context.SaveChanges();
                            return Json(new { Message = "Some error occurred!", Success = false });
                        }
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
                            UserManager.RemoveFromRole(m.UserId, OldRoleName);
                            var r = UserManager.AddToRole(m.UserId, u.RoleId);
                        }
                        m.ExpWhenJoined = u.ExpWhenJoined;
                        m.PhoneNumber = u.PhoneNumber;
                        m.AlternatePhoneNumber = u.AlternatePhoneNumber;
                        m.PersonalEmailAddress = u.PersonalEmailAddress;
                        m.FatherName = u.FatherName;
                        m.MotherName = u.MotherName;
                        m.MarritalStatus = u.MarritalStatus;
                        m.SpouseName = u.SpouseName;
                        m.BloodGroup = u.BloodGroup;
                        m.DepartmentId = u.DepartmentId;
                        m.DOB = u.DOB;
                        m.AnniversaryDate = u.AnniversaryDate;
                        m.DocumentDOB = u.DocumentDOB;
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
                        m.PreviousSalary = u.PreviousSalary;
                        m.Salary = u.Salary;
                        m.Increment = u.Increment;
                        m.Vaccination1stDoseDate = u.Vaccination1stDoseDate;
                        m.Vaccination2ndDoseDate = u.Vaccination2ndDoseDate;
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
                        //var random = new Random();
                        //random.Next();
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
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
        public ActionResult DeleteUser(string UserId)
        {
            try
            {
                var ud = _context.UserDetails.Where(x => x.UserId == UserId).ToList();
                var cl = _context.Clients.Where(x => x.CreatedBy == UserId).ToList();
                var pr = _context.Projects.Where(x => x.CreatedBy == UserId).ToList();
                var w = _context.Workings.Where(x => x.CreatedBy == UserId).ToList();
                var doc = _context.UserDocuments.Where(x => x.UserId == UserId).ToList();
                var er = _context.ErrorLogs.Where(x => x.CreatedBy == UserId).ToList();
                var edu = _context.UserEducations.Where(x => x.UserId == UserId).ToList();
                var inc = _context.UserIncrements.Where(x => x.UserId == UserId).ToList();
                var d = _context.AspNetUsers.Where(x => x.Id == UserId).ToList();
                if (UserId == "62ab2752-d0c8-4be7-ba16-59806c303cc2")
                {
                    return Json(new { Message = "You cant'delete this id!", Success = true }, JsonRequestBehavior.AllowGet);
                }
                if (w != null)
                {
                    _context.Workings.RemoveRange(w);
                    _context.SaveChanges();
                }
                if (pr != null)
                {
                    _context.Projects.RemoveRange(pr);
                    _context.SaveChanges();
                }
                if (cl != null)
                {
                    _context.Clients.RemoveRange(cl);
                    _context.SaveChanges();
                }
                if (doc != null)
                {
                    var projectDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                    System.IO.DirectoryInfo di = new DirectoryInfo(projectDir + "\\Documents");

                    foreach (var item in doc)
                    {
                        foreach (FileInfo file in di.GetFiles())
                        {
                            if (item.Name == file.Name)
                            {
                                file.Delete();
                            }
                        }
                    }
                    _context.UserDocuments.RemoveRange(doc);
                    _context.SaveChanges();
                }
                if (er != null)
                {
                    _context.ErrorLogs.RemoveRange(er);
                    _context.SaveChanges();
                }
                if (edu != null)
                {
                    _context.UserEducations.RemoveRange(edu);
                    _context.SaveChanges();
                }
                if (inc != null)
                {
                    _context.UserIncrements.RemoveRange(inc);
                    _context.SaveChanges();
                }
                if (ud != null)
                {
                    _context.UserDetails.RemoveRange(ud);
                    _context.SaveChanges();
                }
                if (d != null)
                {
                    _context.AspNetUsers.RemoveRange(d);
                    _context.SaveChanges();
                }
                return Json(new { Message = "User deleted successfully!", Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        [HttpPost]
        public ActionResult SaveEducation(UserViewModel userViewModel)
        {
            try
            {
                var Success = false;
                var Message = "";
                UserEducation ud = new UserEducation()
                {
                    UserId = userViewModel.UserId,
                    CourseId = userViewModel.CourseId,
                    Percentage = userViewModel.Percentage,
                    YearPassed = userViewModel.YearPassed,
                    Comment = userViewModel.Comment,
                    Id = userViewModel.Id,
                };
                if (userViewModel.Id == 0)
                {
                    var exists = _context.UserEducations.Where(x => x.UserId == userViewModel.UserId && x.CourseId == userViewModel.CourseId).FirstOrDefault();
                    if (exists == null)
                    {
                        using (var dbCtx = new ProjectManagementEntities())
                        {
                            dbCtx.Entry(ud).State = EntityState.Added;
                            dbCtx.SaveChanges();
                            Success = true;
                            Message = "Education added successfully";
                        }
                    }
                    else
                    {
                        return Json(new { Message = "Already exist", Success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var exists = _context.UserEducations.Where(x => x.UserId == userViewModel.UserId && x.CourseId == userViewModel.CourseId && x.Id != userViewModel.Id).FirstOrDefault();
                    if (exists == null)
                    {
                        using (var dbCtx = new ProjectManagementEntities())
                        {

                            dbCtx.Entry(ud).State = EntityState.Modified;
                            dbCtx.SaveChanges();
                            Success = true;
                            Message = "Education updated successfully";
                        }
                    }
                    else
                    {
                        return Json(new { Message = "Already exist", Success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Message = Message, Success = Success }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult SaveIncrement(UserViewModel userViewModel)
        {
            try
            {
                var Success = false;
                var Message = "";
                UserIncrement ud = new UserIncrement()
                {
                    UserId = userViewModel.UserId,
                    DateOfIncrement = userViewModel.DateOfIncrement,
                    Salary = userViewModel.Salary,
                    Id = userViewModel.Id,
                };
                var salary = 0;
                var date = new DateTime();

                var count = 0;
                if (userViewModel.Id == 0)
                {
                    count = _context.UserIncrements.Where(x => x.UserId == userViewModel.UserId).Count();
                }
                else
                {
                    count = _context.UserIncrements.Where(x => x.UserId == userViewModel.UserId && x.Id != userViewModel.Id).Count();
                }
                if (count > 0)
                {
                    var i = _context.UserIncrements.Where(x => x.UserId == userViewModel.UserId).ToList();
                    var s = i.TakeWhile(x => x.Id != userViewModel.Id).Select(x => x.Salary).LastOrDefault();
                    var d = i.TakeWhile(x => x.Id != userViewModel.Id).Select(x => x.DateOfIncrement).LastOrDefault();
                    if (s == null)
                    {
                        var i1 = _context.UserDetails.Where(x => x.UserId == userViewModel.UserId).ToList();
                        var s1 = i1.Select(x => x.Salary).FirstOrDefault();
                        var d1 = i.SkipWhile(x => x.Id != userViewModel.Id).Skip(1).Select(x => x.DateOfIncrement).FirstOrDefault();
                        var next = i.SkipWhile(x => x.Id != userViewModel.Id).Skip(1).Select(x => x.Salary).FirstOrDefault();
                        salary = Convert.ToInt32(s1);
                        var date1 = Convert.ToDateTime(d1);
                        var nextSalary = Convert.ToInt32(next);
                        if (date1 < userViewModel.DateOfIncrement)
                        {
                            return Json(new { Message = "Date of Increment should be less than next increment date", Success = false }, JsonRequestBehavior.AllowGet);
                        }
                        if (nextSalary <= userViewModel.Salary)
                        {
                            return Json(new { Message = "Current Salary should be less than next increment salary", Success = false }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        date = Convert.ToDateTime(d);
                        salary = Convert.ToInt32(s);
                    }
                }
                else
                {
                    var i = _context.UserDetails.Where(x => x.UserId == userViewModel.UserId).ToList();
                    var s = i.Select(x => x.Salary).FirstOrDefault();
                    salary = Convert.ToInt32(s);
                }
                if (userViewModel.Id == 0)
                {
                    var exists = _context.UserIncrements.Where(x => x.UserId == userViewModel.UserId && x.DateOfIncrement == userViewModel.DateOfIncrement).FirstOrDefault();
                    if (exists == null)
                    {
                        if (salary < userViewModel.Salary)
                        {
                            if (count > 0)
                            {
                                if (date > userViewModel.DateOfIncrement)
                                {
                                    return Json(new { Message = "Date of Increment should be greater than previous date of increment", Success = false }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            using (var dbCtx = new ProjectManagementEntities())
                            {
                                dbCtx.Entry(ud).State = EntityState.Added;
                                dbCtx.SaveChanges();
                                Success = true;
                                Message = "Increment added successfully";
                            }
                        }
                        else
                        {
                            return Json(new { Message = "Current salary should be greater than previous salary", Success = false }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { Message = "Already exist", Success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var exists = _context.UserIncrements.Where(x => x.UserId == userViewModel.UserId && x.DateOfIncrement == userViewModel.DateOfIncrement && x.Id != userViewModel.Id).FirstOrDefault();
                    if (exists == null)
                    {
                        if (salary < userViewModel.Salary)
                        {
                            if (count > 0)
                            {
                                if (date > userViewModel.DateOfIncrement)
                                {
                                    return Json(new { Message = "Date of Increment should be greater than previous date of increment", Success = false }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            using (var dbCtx = new ProjectManagementEntities())
                            {
                                dbCtx.Entry(ud).State = EntityState.Modified;
                                dbCtx.SaveChanges();
                                Success = true;
                                Message = "Increment updated successfully";
                            }
                        }
                        else
                        {
                            return Json(new { Message = "Current salary should be greater than previous salary", Success = false }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { Message = "Already exist", Success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Message = Message, Success = Success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ActionResult DeleteEducation(int Id)
        {
            var d = _context.UserEducations.SingleOrDefault(x => x.Id == Id); //returns a single item.
            if (d != null)
            {
                _context.UserEducations.Remove(d);
                _context.SaveChanges();
            }
            return Json(new { Message = "Education deleted successfully!", Success = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteIncrement(int Id)
        {
            var d = _context.UserIncrements.SingleOrDefault(x => x.Id == Id); //returns a single item.
            if (d != null)
            {
                _context.UserIncrements.Remove(d);
                _context.SaveChanges();
            }
            return Json(new { Message = "Increment deleted successfully!", Success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}