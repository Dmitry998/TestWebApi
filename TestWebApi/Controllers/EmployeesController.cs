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


            //return await db.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeesThisCompany(int id)
        {
            //Employee employee = await db.Employees.FirstOrDefaultAsync(x => x.Id == id);
            //if (employee == null)
            //    return NotFound();


            //Passport passport = await db.Passports.Include(p => p.Employee).FirstOrDefaultAsync(x => x.Id == id);
            //if (passport == null)
            //    return NotFound();
            //return new ObjectResult(passport);

            //List<Passport> passportsThisEmployeeId = new List<Passport>();

            //foreach(var p in await db.Passports.Include(p => p.Employee.Company).ToListAsync())
            //{
            //    if (p.EmployeeId == id)
            //        passportsThisEmployeeId.Add(p);
            //}
            //if (passportsThisEmployeeId.Count == 0)
            //    return NotFound();
            //return Ok(passportsThisEmployeeId);


            List<Employee> employeesThisCompany = new List<Employee>();
            foreach (Employee em in await db.Employees
                .Include(e => e.Passports)
                .ToListAsync())
            {
                if (em.CompanyId == id)
                    employeesThisCompany.Add(em);
            }
            if (employeesThisCompany.Count == 0)
                return NotFound();
            return Ok(employeesThisCompany);
        }


        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmpolyee(Employee employee)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            db.Employees.Add(employee);
            await db.SaveChangesAsync();
            return Ok(employee);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Employee>> GetNameCompany(int id)
        //{
        //    //Employee employee = await db.Employees.FirstOrDefaultAsync(x => x.Id == id);
        //    //if (employee == null)
        //    // return NotFound();

        //    Company company = await db.Companies.FirstOrDefaultAsync(x => x.Id == id);
        //    if (company == null)
        //        return NotFound();
        //    return new ObjectResult(company);

        //    //List<Employee> employeesThisCompany = new List<Employee>();
        //    //foreach (Employee em in await db.Employees.ToListAsync())
        //    //{
        //    //    if (em.CompanyId == id)
        //    //        employeesThisCompany.Add(em);

        //    //}
        //    //if (employeesThisCompany.Count == 0)
        //    //    return NotFound();
        //    //return Ok(employeesThisCompany);
        //}

        //[HttpPost]
        //public async Task<ActionResult<Company>> GetNameCompany(string name)
        //{
        //    //Company company = await db.Companies.FirstOrDefaultAsync(x => x.Id == id);
        //    //if (company == null)
        //    //    return NotFound();
        //    //return new ObjectResult(company);

        //    List<Employee> employeesThisCompany = new List<Employee>();
        //    var employees = db.Employees.Include(e => e.Company);
        //    foreach (var em in employees)
        //    {
        //        if (em.Company.Name == name)
        //            employeesThisCompany.Add(em);
        //    }
        //    if (employeesThisCompany.Count == 0)
        //        return NotFound();
        //    return Ok(employeesThisCompany);
        //}

    }
}
