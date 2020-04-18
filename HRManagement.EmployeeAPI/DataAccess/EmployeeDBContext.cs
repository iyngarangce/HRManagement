using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HRManagement.Model.Employee;

namespace HRManagement.EmployeeAPI.DataAccess
{
    public class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options) 
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(new Employee() { Id = 1, Name = "Dhinesh", BasicPay = 15, EmployeeType = Helper.EmployeeType.Permanent });
            modelBuilder.Entity<Employee>().HasData(new Employee() { Id = 2, Name = "Viji", BasicPay = 20, EmployeeType = Helper.EmployeeType.Permanent });
            modelBuilder.Entity<Employee>().HasData(new Employee() { Id = 3, Name = "Dhanush", BasicPay = 10, EmployeeType = Helper.EmployeeType.Temporary });
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
