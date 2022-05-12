using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digisoft.ProjectManagement.Models
{
    public class BaseModel
    {
        public bool IsCurrentUser { get; set; }
        public string ViewType { get; set; }
        public bool IsAdmin { get; } = Utility.StaticMethods.isAdmin();
    }
}