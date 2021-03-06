﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
    public class Employee
    {
        [Key]
        [Display(Name = "Employee ID")]
        public int employeeID { get; set; }
        [Display(Name = "First Name ")]
        public string firstName { get; set; }
        [Display(Name = "Last Name ")]
        public string lastName { get; set; }
        [Display(Name = "Assigned Zip Code")]
        public int AssignedZipCode { get; set; }
        public string userID { get; set; }

    }
}