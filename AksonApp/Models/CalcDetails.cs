using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AksonApp.Models
{
    public class CalcDetails
    {
        public decimal months { get; set; }
        public decimal ballon { get; set; }
        public decimal deposit { get; set; }
        public decimal purchasePrice { get; set; }
        public decimal prime { get; set; }
        [DisplayFormat(DataFormatString = "0:n2",ApplyFormatInEditMode = true)]
        public decimal ballomPrice { get; set; }
        [DisplayFormat(DataFormatString = "0:n2", ApplyFormatInEditMode = true)]
        public decimal Installment { get; set; }
    }
}