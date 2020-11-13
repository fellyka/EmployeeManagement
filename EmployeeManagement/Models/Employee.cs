using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage ="Name should not contain more than 30 characters")]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public string Department { get; set; }
    }
}
