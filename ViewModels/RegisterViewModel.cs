using EmployeeManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action:"IsEmailInUse", controller:"Account")]
        [ValidEmailDomain(allowedDomain: "sollers.co.za", ErrorMessage = "Email domain to be sollers.co.za")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)] /* To Mask characters */
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage = "Password and confirmation Password do not match\nPlease Try again")]
        public string ConfirmPassword { get; set;}
    }
}
