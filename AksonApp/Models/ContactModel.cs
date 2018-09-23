using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AksonApp.Models
{
    public class ContactModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        [StringLength(500,MinimumLength = 20,ErrorMessage = "Minimum message lenght is 20.")]
        public string Message { get; set; }
    }
}