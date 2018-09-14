using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AksonApp.Models
{
    public class LikeTable
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PostId { get; set; }
    }
}