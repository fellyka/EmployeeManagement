using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeCreateViewModel
    {
       // public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name should not contain more than 30 characters")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Dept? Department { get; set; }

        public List<IFormFile> Photos { get; set; } //List<T> to allow multiple files to be uploaded at the same time 
    }
}
