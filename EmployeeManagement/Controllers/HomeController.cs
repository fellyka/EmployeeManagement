using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository repo;
        private readonly IWebHostEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository repo, IWebHostEnvironment hostingEnvironment)
        {
            this.repo = repo;
            this.hostingEnvironment = hostingEnvironment;
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
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if(model.Photo != null)
                {
                    //WebRootPath property provides us the absolute path to the wwwroot folder where we'll upload the images
                  string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                  uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName; //Allows file to have a unique name
                  string filePath =  Path.Combine(uploadFolder,uniqueFileName);
                    //CopyTo file is a method  from IFormFile that copies the contents of the uploaded file to the target stream -- images folder in wwwroot in our case.
                    model.Photo.CopyTo(new FileStream(filePath,FileMode.Create));
                }

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                repo.Add(newEmployee);
                return RedirectToAction("Details", new { id = newEmployee.Id });
            }

            return View();
        }
    }
}
