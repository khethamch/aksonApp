using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AksonApp.Models
{
    public class Products
    {
        public Guid Id { get; set; }
        [Display(Name = "Product")]
        public string ProductName { get; set; }
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal ProductPrice { get; set; }
        [Display(Name = "Image")]
        public string ProductImage { get; set; }
        [Display(Name = "Rating")]
        public int ProductRating { get; set; }
        [Display(Name = "Avaliable")]
        public int StockAvaliable { get; set; }
        [Display(Name = "Remaining")]
        public int StockRemaining { get; set; }
        [Display(Name = "Sold")]
        public int StockSold { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public bool IsCancelled { get; set; }

        public Products()
        {
            Id = Guid.NewGuid();
        }
    }
}