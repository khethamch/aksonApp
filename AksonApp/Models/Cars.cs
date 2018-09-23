using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AksonApp.Models
{
    public class Cars
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Model (e.g BMW)")]
        public string Model { get; set; }
        [Required]
        [Display(Name = "Model (e.g 4 series)")]
        public string Make { get; set; }
        [Display(Name = "Is the car brand new?")]
        public bool IsNew { get; set; }
        [Display(Name = "Exterior Image")]
        public string OutsideImage { get; set; }
        [Display(Name = "Interior Image")]
        public string InteriorImage { get; set; }
        [Required]
        [Display(Name = "Price (ZAR)")]
        [DataType(DataType.Currency)]
        public decimal CarPrice { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "The year should be four digits long.")]
        public string Year { get; set; }
        [Required]
        [Display(Name = "Trasmission Type")]
        public string TransmissionType { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public bool IsCancelled { get; set; }
        [Display(Name = "Is this vehicle for hire?")]
        public bool IsForHire { get; set; }

        public Cars()
        {
            Id = Guid.NewGuid();
            DateTimeStamp = DateTime.Now;
        }
    }
}