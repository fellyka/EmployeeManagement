using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository repo;

        public HomeController(IEmployeeRepository repo)
        {
            this.repo = repo;
        }

      
        public IActionResult Index()
        {
            IEnumerable<Employee> emp = repo.GetAllEmployee();
            return View(emp);
        }

        public IActionResult Details(int? id)
        {
            HomeDetailsViewModels homeDetailsViewModels = new HomeDetailsViewModels()
            {
                Employee = repo.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"

            };
           return View(homeDetailsViewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee newEmployee = repo.Add(employee);
                return RedirectToAction("Details", new { id = newEmployee.Id });
            }

            return View();
        }
    }
}
