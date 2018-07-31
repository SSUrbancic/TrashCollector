using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
    public class TrashPickUp
    {
        [Key]
        [Display(Name = "Pick Up ID")]
        public int pickUpID { get; set; }
        [Display(Name = "Date")]
        public string date { get; set; }
        [Display(Name = "Day Of The Week")]
        public string dayOfWeek { get; set; }
        [Display(Name = "Pick Up Completed")]
        public bool pickUpCompleted { get; set; }
        [Display(Name = "Price")]
        public double price { get; set; } 
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
    }
}
