using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AksonApp.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string KeyWord { get; set; }
        public string Author { get; set; }
        public string Post { get; set; }
    }
}