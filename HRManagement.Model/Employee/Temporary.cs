using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using HRManagement.Helper;

namespace HRManagement.Model.Employee
{
    public class Temporary : Employee
    {       
        public override decimal CalculateSalary()
        {
            return (decimal)1.5 * BasicPay;
        }
    }
}
