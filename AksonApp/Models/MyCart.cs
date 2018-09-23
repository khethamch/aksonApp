using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AksonApp.Models
{
    public class MyCart
    {
        public Guid Id { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Reference { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string Status { get; set; }
        public Guid productId { get; set; }
        public string Image { get; set; }
    }
}