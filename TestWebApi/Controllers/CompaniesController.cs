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
    public class CompaniesController : ControllerBase
    {
        EmployeesContext db;

        public CompaniesController(EmployeesContext context)
        {
            db = context;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetAllCompanies()
        {
            var companies = await db.Companies
                .Include(c => c.Employees)
                .ThenInclude(employee => employee.Passports)
                .ToListAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetThisCompany(int id) // Вся информация о выбранной компании
        {
            Company thisCompany = await db.Companies
                .Include(c => c.Employees)
                .ThenInclude(employee => employee.Passports)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (thisCompany == null)
                return NotFound();

            return Ok(thisCompany);
        }

        [HttpPost]
        public async Task<ActionResult<Company>> AddCompany(Company company)
        {
            if (!db.Companies.Any(c => c.Id == company.Id))
                return BadRequest();

            db.Companies.Add(company);
            await db.SaveChangesAsync();
            return Ok(company);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(int id)
        {
            Company company = db.Companies.FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            db.Companies.Remove(company);
            await db.SaveChangesAsync();
            return Ok(company.Id);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Company>> ChangeCompany(int id, [FromBody] JsonPatchDocument<Company> patch)
        {
            Company company = await db.Companies
                .Include(c => c.Employees)
                .ThenInclude(employee => employee.Passports)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null)
                return NotFound();
            patch.ApplyTo(company, ModelState);
            await db.SaveChangesAsync();
            return Ok(company);
        }
    }
}
