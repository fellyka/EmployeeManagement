using EmployeeManagement.Models;
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

        public string Details(int id)
        {
             return repo.GetEmployee(1).Name;
        }
    }
}
