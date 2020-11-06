using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        EmployeesContext db;

        public EmployeesController(EmployeesContext context)
        {
            db = context;

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
        {
            //var players = db.Players.Include(p => p.Team); это не от сюда даже


            var employees = await db.Employees
                .Include(e => e.Passports)
                .ToListAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeesThisCompany(int id)
        {
            Company thisCompany = await db.Companies.FirstOrDefaultAsync(c => c.Id == id);

            if (thisCompany == null)
                return NotFound();

            List<Employee> employeesThisCompany = new List<Employee>();
            foreach (Employee em in await db.Employees
                .Include(e => e.Passports)
                .ToListAsync())
            {
                if (em.CompanyId == id)
                    employeesThisCompany.Add(em);
            }
            return Ok(employeesThisCompany);
        }


        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmpolyee(Employee employee)
        {
            if (employee == null)
                return BadRequest();

            db.Employees.Add(employee);
            await db.SaveChangesAsync();
            return Ok(employee.Id); // возвращаем id добавленного пользователя
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            Employee employee = db.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            db.Employees.Remove(employee);
            await db.SaveChangesAsync();
            return Ok(employee.Id); // возвращаем id удаленного пользователя.
        }

        [HttpPut]
        public async Task<ActionResult<Employee>> Put(Employee employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }

            if (!db.Employees.Any(e => e.Id == employee.Id))
            {
                return NotFound();
            }

            Employee employeeInDb = db.Employees.FirstOrDefault(e => e.Id == employee.Id);

            if (employee.Name == null)
                employee.Name = employeeInDb.Name;
            if (employee.Surname == null)
                employee.Surname = employeeInDb.Surname;
            if (employee.Phone == null)
                employee.Phone = employeeInDb.Phone;
            if (employee.CompanyId == null)
                employee.CompanyId = employeeInDb.CompanyId;

            db.Update(employee);
            await db.SaveChangesAsync();
            return Ok(employee);
        }

    }
}
