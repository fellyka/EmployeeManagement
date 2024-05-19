using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Name cannot exceed 20 characters")]
        public string? Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-A-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        [Display(Name = "Office Email")]
        public string? Email { get; set; }

        // public string Department { get; set; }

        [Required]
        //Composition
        public Dept? Department { get; set; }

        public string? PhotoPath {  get; set; }

        
    }
}
