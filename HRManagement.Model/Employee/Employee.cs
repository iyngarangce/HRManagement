﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

using HRManagement.Helper;

namespace HRManagement.Model.Employee
{

    [DataContract]
    public class Employee : Payroll
    {
        [Required]
        public string Name { get; set; }
        [Key]
        public int Id { get; set; }

        public int Age { get; set; }

        [Required]
        public Designation Designation { get; set; }

        [Required]
        public decimal BasicPay { get; set; }

        [Required]
        public EmployeeType EmployeeType { get; set; }

        public override decimal CalculateSalary()
        {
            return 2 * BasicPay;
        }
    }
}
