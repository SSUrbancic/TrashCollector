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
        public int pickUpID { get; set; }
        public int customerID { get; set; }
        public DateTime date { get; set; }
        public DayOfWeek dayOfWeek { get; set; }
        public bool pickUpCompleted { get; set; }
        public double price { get; set; }
        
    }
}