using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
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
            var employees = await db.Employees
                .Include(e => e.Passports)
                .ToListAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetThisEmployee(int id) // Получить сотрудника по id
        {
            Employee thisEmployee = await db.Employees
                .Include(e => e.Passports)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (thisEmployee == null)
                return NotFound();

            return Ok(thisEmployee);
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

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Employee>> ChangeThisEmployee(int id, [FromBody] JsonPatchDocument<Employee> patch)
        {
            var employee = await db.Employees
                .Include(e => e.Passports)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
                return NotFound();
            patch.ApplyTo(employee, ModelState);
            await db.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpPut]
        public async Task<ActionResult<Employee>> ChangeEmployeeTotaly(Employee employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }

            if (!db.Employees.Any(e => e.Id == employee.Id))
            {
                return NotFound();
            }

            db.Update(employee);
            await db.SaveChangesAsync();
            return Ok(employee);
        }

    }
}
