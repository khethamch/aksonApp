using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AksonApp.Models
{
    public class CarHireModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Contact")]
        public string Contact { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [Required]
        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }
        [Display(Name = "Price (ZAR)")]
        [DataType(DataType.Currency)]
        public decimal RentalPrice { get; set; }
        public int Duration { get; set; }
        [Display(Name = "Referrence")]
        public string Referrence { get; set; }
        public Guid CarId { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public bool IsCancelled { get; set; }
        public string PayMethode { get; set; }
        public string PickUpLocation { get; set; }
        public bool Accepted { get; set; }
        public bool Navigator { get; set; }
        public bool Bluetooth { get; set; }

        public CarHireModel()
        {
            Id = Guid.NewGuid();
            DateTimeStamp = DateTime.Now;
        }
    }
}