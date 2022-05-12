using Digisoft.ProjectManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Digisoft.ProjectManagement.Attributes
{
    public class ExceptionHandlerAttribute : FilterAttribute,IExceptionFilter
    {
        //protected UserManager<ApplicationUser> UserManager { get; set; }
        private ProjectManagementEntities _context;
        public ExceptionHandlerAttribute()
        {
            _context = new ProjectManagementEntities();
        }
        public void OnException(ExceptionContext filterContext)
        {
            if(!filterContext.ExceptionHandled)
            {
                ErrorLog logger = new ErrorLog();
                //logger.ErrorText = filterContext.Exception.Message;
                logger.ErrorText = filterContext.Exception.StackTrace;
                var controllerName = filterContext.RouteData.Values["controller"].ToString();
                var actionName = filterContext.RouteData.Values["action"].ToString();
                logger.Page = string.Format("/{0}/{1}", controllerName, actionName);
                logger.CreateDate = DateTime.Now;
                logger.CreatedBy = HttpContext.Current.User.Identity.GetUserId();
                
                //save the exception to Db
                _context.ErrorLogs.Add(logger);
                _context.SaveChanges();

            } 
            filterContext.ExceptionHandled = true;

            ////Redirect or return a view, but not both.
            //filterContext.Result = RedirectToAction("Index", "ErrorHandler");
            //// OR 
            filterContext.Result = new ViewResult
            {
                //ViewName = "~/Views/ErrorHandler/Index.cshtml"
                ViewName = "Error"
            };
            //throw new NotImplementedException();
        }

    }
}