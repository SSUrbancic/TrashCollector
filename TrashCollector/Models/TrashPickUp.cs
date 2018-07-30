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
        public int pickUpID { get; set; }
        public string date { get; set; }
        public string dayOfWeek { get; set; }
        public bool pickUpCompleted { get; set; }
        public double price { get; set; } 
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
    }
}
