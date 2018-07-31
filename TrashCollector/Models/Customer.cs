using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Display(Name = "Last Name")]
        public string lastName { get; set; }
        [Display(Name = "Address Line 1")]
        public string addressLine1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string addessLine2 { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "State")]
        public string state { get; set; }
        [Display(Name = "Zip Code")]
        public int zipCode { get; set; }
        [Display(Name = "Payment Balance")]
        public double paymentBalance { get; set; }
        [ForeignKey("ApplicationUser")]
        public string userID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}