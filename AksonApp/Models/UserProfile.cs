using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AksonApp.Models
{
   
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string UserId { get; set; }

        public UserProfile()
        {
            Id = Guid.NewGuid();
            DateTimeStamp = DateTime.Now;
        }
    }
}