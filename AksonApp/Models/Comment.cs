using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AksonApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Display(Name = "Written By")]
        public string Name { get; set; }
        [Display(Name = "Comment")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public Guid BlogId { get; set; }

    }
}