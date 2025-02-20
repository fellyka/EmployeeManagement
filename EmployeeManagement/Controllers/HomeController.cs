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

        public ActionResult Details(int id)
        {
            Employee model = _employeeRepository.GetEmployee(5);
            return View(model);
        }
    }
}
