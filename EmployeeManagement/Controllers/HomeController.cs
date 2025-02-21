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
           
            /*Let us use ViewData : It's a Dictionary of weakly typed objects that uses string keys to store and retrieve data*/
            Employee model = _employeeRepository.GetEmployee(7);
            /*The Key here is Employee for the model, and PageTitle for the Employee Details, which is the title of our page*/
            ViewData["Employee"] = model;
            ViewData["PageTitle"] = "Employee Details";
            return View();
        }
    }
}
