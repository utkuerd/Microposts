using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Microposts.Models
{
    public class Micropost
    {
        [Index("IdAndCreatedAt",1)]
        public int Id { get; set; }
        [Required]
        [StringLength(140)]
        public string Content { get; set; }
        [Required]
        public virtual ApplicationUser User { get; set; }
        [Index("IdAndCreatedAt",2)]
        public DateTime CreatedAt { get; set; }
        public string Image { get; set; }
    }
}