using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AksonApp.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Total Items")]
        public int TotalItems { get; set; }
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
        public string Reference { get; set; }
        public string UserId { get; set; }
        [Display(Name = "House Number")]
        [Required]
        public int HouseNumber { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        public string Province { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Code should be 4 digits long.")]
        public string Code { get; set; }
        public string Status { get; set; }
    
    }
}