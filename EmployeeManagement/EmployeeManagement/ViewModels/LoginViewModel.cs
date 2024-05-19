using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        /*This RememberMe will create a persistant cookie if the button is checked, otherwise, a session cookie*/
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set;}
    }
}
