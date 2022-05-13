using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Digisoft.ProjectManagement.Models
{
    public class WorkingViewModel : BaseModel
    {
        public int Id { get; set; }
        [Display(Name = "Project")]
        public int ProjectId { get; set; }
        [Display(Name = "Created On")]
        public System.DateTime CreatedDate { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal HoursBilled { get; set; }
        public System.DateTime? DateWorked { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }
        public List<SelectListItem> Projects { get; set; }
        public string Working { get; set; }
        public string ProjectName { get; set; }
        public bool? IsActive { get; set; }
    }

}