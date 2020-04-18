using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using HRManagement.Helper;

namespace HRManagement.Model.Employee
{
    public class Employee
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
        public decimal Salary { get; set; }

        [Required]
        public EmployeeType EmployeeType { get; set; }

        public virtual decimal CalculateSalary()
        {
            return 2 * BasicPay;
        }
    }
}
