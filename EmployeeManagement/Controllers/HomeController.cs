using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public ViewResult Index()
        {
            //throw new Exception("This is an exception");
            ViewBag.Title = "Employee List";    
            var model = _employeeRepository.GetAllEmployees();
            return (View(model));
        }

        public ViewResult Details(int? id)
        {
            /*Different ways of passing data a view from a controller: ViewData, ViewBag and Stronlgy typed View(The one we'll use in our app)*/

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id ?? 10),    
                PageTitle = "Employee Details"
            };
            
            return View(homeDetailsViewModel);
        }
    }
}
