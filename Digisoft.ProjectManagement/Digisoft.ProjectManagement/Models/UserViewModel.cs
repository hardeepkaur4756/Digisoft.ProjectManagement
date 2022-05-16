using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Digisoft.ProjectManagement.Models
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string RoleId { get; set; }
        public string ViewType { get; set; }
        public string UserId { get; set; }
        public bool Exclude { get; set; }
        public System.DateTime? DOB { get; set; }
        public System.DateTime? DateofJoining { get; set; }
        public System.DateTime? DateofRelieving { get; set; }
        public string Skills { get; set; }
        public decimal? Salary { get; set; }
        public string ExpWhenJoined { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonalEmailAddress { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactRelation { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string AadharNumber { get; set; }
        public string PanNumber { get; set; }
        public string CurrentCity { get; set; }
        public int CurrentStateId { get; set; }
        public int CurrentCountryId { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentCity { get; set; }
        public int PermanentStateId { get; set; }
        public int PermanentCountryId { get; set; }
        public int DepartmentId { get; set; }
        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public string DepartmentName { get; set; }
        public string RoleName { get; set; }
        public string PermanentStateName { get; set; }
        public string PermanentCountryName { get; set; }
        public string CurrentStateName { get; set; }
        public string CurrentCountryName { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public List<UserDocument> Documents { get; set; }

    }
    public class UserList
    {
        public List<UserViewModel> lst { get; set; }
    }
}