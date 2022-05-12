using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Digisoft.ProjectManagement.Models
{
    public class UserDocumentViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DocumentName { get; set; }
        public string ContentType { get; set; }
        public string Document { get; set; }
    }
}