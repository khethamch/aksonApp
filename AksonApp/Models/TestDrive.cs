using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AksonApp.Models
{
    public class TestDrive
    {
        public Guid Id { get; set; }
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name*")]
        public string LastName { get; set; }
        [Display(Name = "Contact Number*")]
        public string ContactNumber { get; set; }
        [Display(Name = "Email*")]
        public string Email { get; set; }
        [Display(Name = "Model Of Interest*")]
        public string Model { get; set; }
        [Display(Name = "Make Of Interest*")]
        public DateTime Make { get; set; }
        [Display(Name = "Licence Number*")]
        public string Licence { get; set; }
        [Display(Name = "Attathment (Licence Copy)*")]
        public string Attathment { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public TestDrive()
        {
            Id = Guid.NewGuid();
            DateTimeStamp = DateTime.Now;
        }
    }
}