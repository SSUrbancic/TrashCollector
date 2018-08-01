using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public double Amount { get; set; }
        [ForeignKey("TrashPickUp")]
        public int PickUpID { get; set; }
        public TrashPickUp TrashPickUp { get; set; }
    }
}