using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HRManagement.DTO;
using HRManagement.EmployeeAPI.DataAccess;
using HRManagement.Model.Employee;
using Microsoft.AspNetCore.Mvc;

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
            return Content("Welcome to HR Management System");
        }

        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployee()
        {

            IEnumerable<EmployeeDTO> employeeDTOTemp = await _context.Employees.Where(x => x.EmployeeType == Helper.EmployeeType.Temporary)
                .Select(x => EmployeeToDTO(GetEmployeeType(x) as Temporary))
                .ToListAsync();

            IEnumerable<EmployeeDTO> employeeDTOPerm = await _context.Employees.Where(x => x.EmployeeType == Helper.EmployeeType.Permanent)
                .Select(x => EmployeeToDTO(GetEmployeeType(x) as Employee))
                .ToListAsync();

            return employeeDTOPerm.Union(employeeDTOTemp).ToList();
        }

        [HttpGet("Get/{id:int}")]
        public async Task<ActionResult<EmployeeDTO>> Get(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeDto = CreateEmployeeAsDTO(employee);

            return employeeDto;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<EmployeeDTO>> Add(Employee employeeDTO)
        {
            var employee = CreateDTOAsEmployee(employeeDTO);

            try
            {
                _context.Employees.Add((Employee)employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, ex.StackTrace);
            }

            return Content("Created Successfully...");
        }


        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, Employee employeeDTO)
        {
            if (id != employeeDTO.Id)
            {
                return BadRequest();
            }

            if (!EmployeeExists(id))
            {
                return NotFound();
            }

            var employee = CreateDTOAsEmployee(employeeDTO);
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


        [HttpDelete("Delete/{id:int}")]
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

        private static Object GetEmployeeType(Employee employee) =>

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
            :
                employee;

        private static EmployeeDTO EmployeeToDTO(Employee employee) =>
        new EmployeeDTO
        {
            Id = employee.Id,
            Age = employee.Age,
            Name = employee.Name,
            BasicPay = employee.BasicPay,
            Salary = employee.CalculateSalary(),
            Designation = employee.Designation,
            EmployeeType = employee.EmployeeType
        };

        private static EmployeeDTO EmployeeToDTO(Temporary employee) =>
        new EmployeeDTO
        {
            Id = employee.Id,
            Name = employee.Name,
            Age = employee.Age,
            BasicPay = employee.BasicPay,
            Salary = employee.CalculateSalary(),
            Designation = employee.Designation,
            EmployeeType = employee.EmployeeType
        };



        private bool EmployeeExists(int id) => _context.Employees.Any(e => e.Id == id);


        private EmployeeDTO CreateEmployeeAsDTO(Employee employee)
        {
            EmployeeDTO employeeDto = new EmployeeDTO();
            Temporary templ;

            if (GetEmployeeType(employee) is Temporary)
            {
                templ = GetEmployeeType(employee) as Temporary;
                templ.Salary = templ.CalculateSalary();
                employeeDto = _mapper.Map<EmployeeDTO>(templ);
            }
            else if (GetEmployeeType(employee) is Employee)
            {
                employee.Salary = employee.CalculateSalary();
                employeeDto = _mapper.Map<EmployeeDTO>(employee);
            }

            return employeeDto;
        }

        private Employee CreateDTOAsEmployee(Employee employee)
        {
            if (employee.EmployeeType == Helper.EmployeeType.Permanent)
            {
                employee.Salary = employee.CalculateSalary();
            }
            else if (employee.EmployeeType == Helper.EmployeeType.Temporary)
            {
                var entity = new Temporary();
                entity.BasicPay = employee.BasicPay;
                employee.Salary = entity.CalculateSalary();
            }
            return employee;
        }
    }
}

