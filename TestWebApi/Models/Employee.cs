﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public int? CompanyId { get; set; }

        //public Company Company { get; set; }

        public ICollection<Passport> Passports { get; set; }
        public Employee()
        {
            Passports = new List<Passport>();
        }
    }
}
