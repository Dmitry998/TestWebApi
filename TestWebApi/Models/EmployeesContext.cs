using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestWebApi.Models
{
    public class EmployeesContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }
        public EmployeesContext(DbContextOptions<EmployeesContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
