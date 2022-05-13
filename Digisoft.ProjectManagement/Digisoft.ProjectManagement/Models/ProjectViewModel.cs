using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Digisoft.ProjectManagement.Models
{
    public class ProjectViewModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Created On")]
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }
        [Display(Name = "Client")]
        public int ClientId { get; set; }
        public List<SelectListItem> Clients { get; set; }
        public string ClientName { get; set; }
        public bool? IsActive { get; set; }
    }
}