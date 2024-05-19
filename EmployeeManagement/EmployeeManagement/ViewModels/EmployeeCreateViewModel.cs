using EmployeeManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeCreateViewModel
    {

        [Required]
        [MaxLength(20, ErrorMessage = "Name cannot exceed 20 characters")]
        public string? Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-A-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        [Display(Name = "Office Email")]
        public string? Email { get; set; }

        [Required]
        public Dept? Department { get; set; }

       //public IFormFile? Photo { get; set; }

        /* To support multiple files upload, We have
           set the data type of Photos property to 
        List<IFormFile>  - Verify the Definition of
        IFormFile to see its Properties and Methods
        */
         public List<IFormFile>? Photos { get; set; }

    }
}