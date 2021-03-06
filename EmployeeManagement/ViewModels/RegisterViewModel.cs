﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        //This Remote attribute calls IsEmailInUse action method
        //defined in the AccountController
        [Remote(action:"IsEmailInUse", controller:"Accoount")]
        public string Email{get;set;}

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage ="Password and configuration password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
