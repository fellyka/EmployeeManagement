using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;


namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public string Index()
        {
            var employee = _employeeRepository.GetEmployee(1).Name;
            return employee;
        }

        public IActionResult Details()
        {
            Employee model = _employeeRepository.GetEmployee(1);
            ViewBag.Employee = model;
            ViewBag.PageTitle = "Employee Details";
            return View(model);
        }
    }
}
