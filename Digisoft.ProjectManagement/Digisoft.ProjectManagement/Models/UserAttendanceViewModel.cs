using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Digisoft.ProjectManagement.Models
{
    public class UserAttendanceViewModel : BaseModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public System.DateTime? Date { get; set; }
        public System.TimeSpan? InTime { get; set; }
        public System.TimeSpan? OutTime { get; set; }
        public bool? OnLeave { get; set; }
        public string LeaveApprovedBy { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string CreatedByName { get; set; }
        public string AttendanceStatus { get; set; }
        public List<SelectListItem> Users { get; set; }
        public List<UserDetail> UserDetails  { get; set; }
    }

}