//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Digisoft.ProjectManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserDocument
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DocumentName { get; set; }
        public string ContentType { get; set; }
        public string Document { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
