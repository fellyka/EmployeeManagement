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
            { //Data to be printed on the edit method
                id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };

            return View(employeeEditViewModel);
        }

        [HttpPost]
        /* Through model binding, the action method parameter EmployeeEditViewModel receives the 
           posted edit form data
        */
        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
           /* Verify that the provided data is valid, if not render the edit view */
           if(ModelState.IsValid) 
            {
                /* Retrieve the employee being edited form the db */
                Employee employee = _employeeRepository.GetEmployee(model.id);
                /* Update the employee object with the data in the model object*/
                employee.Name = model.Name;
                employee.Email = model.Email;   
                employee.Department = model.Department;

                /* If the user wants to change the photo, a new photo will be uploaded
                   and the Photo property on the model object receives the uploaded photo
                   If the Photo property is null, user did not upload a new photo and keeps 
                   his existing photo*/

                if(model.Photos != null)
                {
                    /* If a new photo is uploaded, the existing photo must be deleted
                       So check, if there is an existing photo, delete*/
                    if(model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    /* Save the new photo in wwwroot/images folder and update PhotoPath property
                      of the employee object which will be eventually saved in the db */
                    employee.PhotoPath = ProcessUploadedFile(model);
                }

                /* Call update method on the repository service passing in the  employee object
                   to update object to update the data in the database table */
                  Employee updatedEmployee = _employeeRepository.Update(employee);

                return RedirectToAction("index");
            }

            return View(model);
        }

        private string ProcessUploadedFile(EmployeeEditViewModel model)
        {
            string uniqueFileName = null;

            if(model.Photos != null && model.Photos.Count > 0)
            {
                foreach (IFormFile photo in model.Photos)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;/* + new DateTime().ToString();*/
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                }
            }

            return uniqueFileName;
        }
    }//end of HomeController
}//end of namesapce EmployeeManagement
