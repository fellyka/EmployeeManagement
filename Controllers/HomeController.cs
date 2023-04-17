using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository,
            IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }

        public IActionResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id??1),
                PageTitle = "Employee Details"
            };
           
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        /* We could also use IActionResult since RedirectToActionResult inherits from 
           IActionResult 
             public RedirectToActionResult Create(Employee employee) 
        */
        public IActionResult Create(EmployeeCreateViewModel model) 
        {
            if (ModelState.IsValid) //Validation
            {
                string uniqueFileName = null;
                /*If the Photo property on the incoming model object isn't null and if the count > 0,
                 then the user has selected at least one file to upload */
                if(model.Photos != null && model.Photos.Count > 0)
                {
                    //Loop through each selected file
                    foreach (IFormFile photo in model.Photos)
                    {
                        /*The image must be uploaded to the images forlder in wwwroot. 
                          To get the path of the wwwroot folder, we're using the inject
                          HostingEnvironment service provided by ASP.NET Core*/
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                        /*Ensure that the file name is unique - Append a new GUID(Global Unique IDentifier) value
                          and an underscore to the file name*/
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);//Folder + Unique Name

                        /*Use CopyTo() method provided by IformFile interface to copy the file to wwwroot/images folder*/
                        photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    /*Store the file name in PhotoPath property of the employee object
                     which gets saved to the Employees db table*/
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };

            return View(employeeEditViewModel);
        }

    }//end of HomeController
}//end of namesapce EmployeeManagement
