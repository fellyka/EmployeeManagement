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

        public string Details(int id)
        {
            string model = _employeeRepository.GetEmployee(5).Name;
            return model;
        }
    }
}
