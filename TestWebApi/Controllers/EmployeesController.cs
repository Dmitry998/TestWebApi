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
            return await db.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetConcreteEmployee(int id)
        {
            Employee employee = await db.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null)
                return NotFound();
            return new ObjectResult(employee);
        }


    }
}
