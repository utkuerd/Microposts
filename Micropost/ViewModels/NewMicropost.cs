using Microposts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Microposts.ViewModels
{
    public class NewMicropost
    {
        [Required]
        [StringLength(140)]
        public string TweetContent { get; set; }        
        public virtual ApplicationUser User { get; set; }       
        public HttpPostedFileBase File { get; set; }        
        public string objectContext { get; set; }
    }
}