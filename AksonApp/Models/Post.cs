using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AksonApp.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Blurb { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public string Ref { get; set; }

    }
}