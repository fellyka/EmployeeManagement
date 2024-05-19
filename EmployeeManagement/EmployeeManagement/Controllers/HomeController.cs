using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace EmployeeManagement.Controllers
{
    /*By applying [Authorize] attribute at class level, no actions will be reached, unless we're logged in - We'd however able to register since the Register action is 
      defined in a different controller.
     If we want to reach an action while [Authorize] attribute is used, we'll to use [AllowAnonymous] on the desired Actions.
     In addition to the simple [Authorize] attribute that enforces everyone to login, Asp .Net core also supports Role based Authorization,
     Claims based Authorization and Policy based Authorization.
     */
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _repository;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        /*Create a construcor injection to make use of DI*/
        public HomeController(IEmployeeRepository repository, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }
        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _repository.GetAllEmployees();
            return View(model);
        }

        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            /*Generate an error here to help you understand to catch Global exception*/
            //throw new Exception("Error in details view");
            //Employee model  = _repository.GetEmployee(1);
            //ViewBag.PageTitle = "Employee Details";
            Employee employee = _repository.GetEmployee(id.Value);

            if(employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            HomeDetailsViewModels homeDetailsViewModels = new HomeDetailsViewModels()
            {
                /*Instaed of calling GetEmployee twice, let us just use this instance of employee */
               // Employee = _repository.GetEmployee(id??1),//null coalescing operator
                Employee = employee,
                PageTitle = "Employee details"

            };
           // return View(model);
           return View(homeDetailsViewModels);
        }

        [HttpGet]
        /*Not everyone should be able to create a new employee*/
        /*Since we need to be Authorized before performing this action, .Net core will redirect us, if we are Authorized to the Login screen*/
        [Authorize]
        /*This piece of comment applies to all the actions containing the [Authorize] attribute: 
         * This [Authorize] attribute generate an url containing the "...ReturnUrl...", which is used to redirect the user to the page that he
          was trying to access after a succesful login*/
        public ViewResult Create()
        {
            return View();
        }


        [HttpGet]
        /*Not everyone should be able to edit an employee*/
        [Authorize]
        /*Since we need to be Authorized before performing this action, .Net core will redirect us, if we are Authorized to the Login screen*/
        
        public ViewResult Edit(int id)
        {
            Employee employee = _repository.GetEmployee(id);
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
        /* We could also use IActionResult since RedirectToActionResult inherits from 
           IActionResult 
             public RedirectToActionResult Create(Employee employee) 
        */
        [Authorize]
        /*Since we need to be Authorized before performing this action, .Net core will redirect us, if we are Authorized to the Login screen*/
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid) //Validation
            {
                string uniqueFileName = null;
                /* This whole portion of code is commendted because it is the same code found in the HttpPost Create method
                 * and in the HttpPost Edit method. We could have refactored it but it is left here for comments purposes.


                /*If the Photo property on the incoming model object isn't null and if the count > 0,
                 then the user has selected at least one file to upload */
                //  if(model.Photos != null && model.Photos.Count > 0)
                // {
                //Loop through each selected file
                // foreach (IFormFile photo in model.Photos)
                //  {
                /*The image must be uploaded to the images forlder in wwwroot. 
                  To get the path of the wwwroot folder, we're using the inject
                  HostingEnvironment service provided by ASP.NET Core*/
                //  string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                /*Ensure that the file name is unique - Append a new GUID(Global Unique IDentifier) value
                  and an underscore to the file name*/
                // uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                // string filePath = Path.Combine(uploadsFolder, uniqueFileName);//Folder + Unique Name

                /*Use CopyTo() method provided by IformFile interface to copy the file to wwwroot/images folder*/
                //  photo.CopyTo(new FileStream(filePath, FileMode.Create));
                // }
                // }*/

                uniqueFileName = ProcessUploadedFile(model);

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    /*Store the file name in PhotoPath property of the employee object
                     which gets saved to the Employees db table*/
                    PhotoPath = uniqueFileName
                };
                _repository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        /*Since we need to be Authorized before performing this action, .Net core will redirect us, if we are Authorized to the Login screen*/
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            /* Verify that the provided data is valid, if not render the edit view */
            if (ModelState.IsValid)
            {
                /* Retrieve the employee being edited form the db */
                Employee employee = _repository.GetEmployee(model.id);
                /* Update the employee object with the data in the model object*/
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                /* If the user wants to change the photo, a new photo will be uploaded
                   and the Photo property on the model object receives the uploaded photo
                   If the Photo property is null, user did not upload a new photo and keeps 
                   his existing photo*/

                if (model.Photos != null)
                {
                    /* If a new photo is uploaded, the existing photo must be deleted
                       So check, if there is an existing photo, delete*/
                    if (model.ExistingPhotoPath != null)
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
                Employee updatedEmployee = _repository.Update(employee);

                return RedirectToAction("index");
            }

            return View(model);
        }


        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null!;

            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (IFormFile photo in model.Photos)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;/* + new DateTime().ToString();*/
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create)) //using to dispose the fileStream after copying
                    {
                        photo.CopyTo(fileStream);
                    }
                }
            }

            return uniqueFileName;
        }
    }
}
