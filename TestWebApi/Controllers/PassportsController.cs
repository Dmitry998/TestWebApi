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
    public class PassportsController : ControllerBase
    {
        EmployeesContext db;

        public PassportsController(EmployeesContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllPassports()
        {
            var passports = await db.Passports.ToListAsync();
            return Ok(passports);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetPassportsThisEmployee(int id)
        {
            Employee thisEmployee = await db.Employees
                .Include(e => e.Passports)
                .FirstOrDefaultAsync(e => e.Id == id);

            if(thisEmployee == null)
                return NotFound();

            List<Passport> passportsThisEmployee = thisEmployee.Passports.ToList();
            return Ok(passportsThisEmployee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddPasport(Passport passport)
        {
            if (!db.Employees.Any(e => e.Id == passport.EmployeeId)) // Если нет сотрудника с таким id
                return BadRequest();

            db.Passports.Add(passport);
            await db.SaveChangesAsync();
            return Ok(passport); // возвращаем добавленный паспорт
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Passport>> DeletePassport(int id)
        {
            Passport passport = db.Passports.FirstOrDefault(e => e.Id == id);
            if (passport == null)
            {
                return NotFound();
            }
            db.Passports.Remove(passport);
            await db.SaveChangesAsync();
            return Ok(passport); // возвращаем удаленный паспорт.
        }
    }
}
