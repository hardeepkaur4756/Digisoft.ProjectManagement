using Digisoft.ProjectManagement.Attributes;
using System.Web;
using System.Web.Mvc;

namespace Digisoft.ProjectManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionHandlerAttribute());
            //filters.Add(new HandleErrorAttribute());
        }
    }
}
