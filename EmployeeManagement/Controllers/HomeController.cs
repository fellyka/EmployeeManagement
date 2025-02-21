using EmployeeManagement.Models;

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
        public string Index()
        {
            return "";
        }

        public ViewResult Details(int id)
        {
            /*Different ways of passing data a view from a controller: ViewData, ViewBag and Stronlgy typed View(The one we'll use in our app)*/
           
            /*Let us use ViewBag : It's a Wrapper around ViewData*/
            Employee model = _employeeRepository.GetEmployee(7);
            
            ViewBag.Employee = model;
            ViewBag.PageTitle = "Employee Details";
            return View();
        }
    }
}
