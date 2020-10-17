using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HRManagement.DTO;
using HRManagement.EmployeeAPI.DataAccess;
using HRManagement.Model.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRManagement.EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(EmployeeDBContext context, IMapper mapper, ILogger<EmployeeController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        // GET: /<controller>/
        [HttpGet("Index")]
        public IActionResult Index()
        {
            _logger.LogInformation("Hi, Is the serilog Working");
            return Content("Welcome to HR Management System");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> Get()
        {

            IEnumerable<EmployeeDTO> employeeDTO = await _context.Employees
                .Select(x => GetEmployeeDTO(GetEmployee(x)))
                .ToListAsync();

            return employeeDTO.ToList();
        }
         
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeDTO>> Get(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeDto = GetEmployeeDTO(GetEmployee(employee));

            return employeeDto;
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> Post(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, ex.StackTrace);
            }

            return Content("Created Successfully...");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            if (!EmployeeExists(id))
            {
                return NotFound();
            }
            //_context.Entry<Employee>(employee).State = EntityState.Detached;

            try
            {
                _context.Update<Employee>(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!EmployeeExists(id))
            {
                return NotFound();
            }

            return Content("Updated Successfully...");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, ex.StackTrace);
            }

            return Content("Deleted Successfully...");
        }

        private static Employee GetEmployee(Employee employee) =>

            (employee.EmployeeType == Helper.EmployeeType.Temporary) ?
                new Temporary()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    BasicPay = employee.BasicPay,
                    Age = employee.Age,
                    Designation = employee.Designation,
                    EmployeeType = employee.EmployeeType
                }
            : new Employee()
            {
                Id = employee.Id,
                Name = employee.Name,
                BasicPay = employee.BasicPay,
                Age = employee.Age,
                Designation = employee.Designation,
                EmployeeType = employee.EmployeeType
            };

        private static EmployeeDTO GetEmployeeDTO(Employee employee) =>
            new EmployeeDTO()
            {
                Id = employee.Id,
                Name = employee.Name,
                BasicPay = employee.BasicPay,
                Age = employee.Age,
                Salary = employee.CalculateSalary(),
                Designation = employee.Designation,
                EmployeeType = employee.EmployeeType
            };

        private bool EmployeeExists(int id) => _context.Employees.Any(e => e.Id == id);   
    }
}

