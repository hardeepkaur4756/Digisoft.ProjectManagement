using System.ComponentModel.DataAnnotations;

namespace Digisoft.ProjectManagement.Models
{
    public class ClientViewModel:BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Created On")]
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }
        public bool? IsActive { get;  set; }
    }
}