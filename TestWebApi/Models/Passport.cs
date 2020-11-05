using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.Models
{
    public class Passport
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        //public Employee Employee { get; set; }

        public string Type { get; set; }
        public string Number { get; set; }
    }
}
