using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AksonApp.Models
{
    public class Bookings
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
        [Display(Name = "Vehicle Registration*")]
        public string CarRegistration { get; set; }
        [Display(Name = "First Date Option*")]
        public DateTime FirstDateOption { get; set; }
        [Display(Name = "Second Date Option*")]
        public DateTime SecondDateOption { get; set; }
        [Display(Name = "Drop Off Time*")]
        public DateTime DropOffTime { get; set; }
        [Display(Name = "Do you need to be dropped off?")]
        public bool DropOff { get; set; }
        [Display(Name = "Do you need to be picked up?")]
        public bool PickUp { get; set; }
        [Display(Name = "Service Type*")]
        public string ServiceType { get; set; }
        [Display(Name = "Other")]
        public string Other { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public Bookings()
        {
            Id = Guid.NewGuid();
            DateTimeStamp = DateTime.Now;
        }
    }
}