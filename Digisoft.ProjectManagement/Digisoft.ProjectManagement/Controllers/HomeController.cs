using Digisoft.ProjectManagement.Attributes;
using Digisoft.ProjectManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace Digisoft.ProjectManagement.Controllers
{
    [ExceptionHandler]
    public class HomeController : Controller
    {
        private ProjectManagementEntities _context;
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public HomeController()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            _context = new ProjectManagementEntities();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

        [Authorize]
        public ActionResult Index()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                return View("User");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #region User Dashboard  
       
        #endregion

        #region Admin dashboard
      
        #endregion
    }
}