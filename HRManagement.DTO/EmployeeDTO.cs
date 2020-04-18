using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using HRManagement.Helper;
using Newtonsoft.Json;

namespace HRManagement.DTO
{
    [DataContract]
    public class EmployeeDTO
    {
        public string Name { get; set; }
        public int Id { get; set; }
        
        public int Age { get; set; }
        
        public Designation Designation { get; set; }
        
        public decimal BasicPay { get; set; }
        
        public decimal Salary { get; set; }
        
        public EmployeeType EmployeeType { get; set; }
    }
}
