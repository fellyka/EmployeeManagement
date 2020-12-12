using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
           // throw new Exception("Error in Details");  //-- used to simulate an exception, to be removed in production release 
            Employee employee = repo.GetEmployee(id.Value);

            if(employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            HomeDetailsViewModels homeDetailsViewModels = new HomeDetailsViewModels()
            {
                Employee = employee,
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
                //ProcessUploadFile() is a refactored method. Go to is definition to for more info
                string uniqueFileName = ProcessUploadedFile(model);   
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = repo.GetEmployee(id); //find data to be edited
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel  //edit data
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = repo.GetEmployee(model.Id);
                //Updating the data. The Id is mapped with the hidden file in the Edit view(HttpGet)
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                //ProcessUploadFile() is a refactored method to avaoid code duplication.
                //Go to is definition to for more info

                if (model.Photos != null)
                {
                    if(model.ExistingPhotoPath != null) //Does the user have an existing photo?
                    {
                        //Get the physical root of photo with WebRootPath using IWebHostingEnvironment object
                       string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath); //After finding the path with the line above, delete the photo
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }

                repo.Update(employee);
                return RedirectToAction("Index");
            }

            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (IFormFile photo in model.Photos)
                {

                    //WebRootPath property provides us the absolute path to the wwwroot folder where we'll upload the images
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName; //Allows file to have a unique name
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    //CopyTo file is a method  from IFormFile that copies the contents of the uploaded file to the target stream -- images folder in wwwroot in our case.

                    //Properly dispose the FileStream to avoid error
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                       
                }
            }

            return uniqueFileName;
        }
    }
}
