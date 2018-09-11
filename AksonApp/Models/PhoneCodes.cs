using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AksonApp.Models
{
    public class PhoneCodes
    {
        public Guid Id { get; set; }
        public string code { get; set; }

        public PhoneCodes()
        {
            Id = Guid.NewGuid();
        }
    }
}