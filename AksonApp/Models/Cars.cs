using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AksonApp.Models
{
    public class Cars
    {
        public Guid Id { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public bool IsNew { get; set; }
        public string OutsideImage { get; set; }
        public string InteriorImage { get; set; }
        public decimal CarPrice { get; set; }
        public string Year { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public bool IsCancelled { get; set; }

        public Cars()
        {
            Id = Guid.NewGuid();
        }
    }
}