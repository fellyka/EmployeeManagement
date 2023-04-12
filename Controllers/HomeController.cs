using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace EmployeeManagement.Controllers
{
    public class HomeController
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public string Index()
        {
            var employee = _employeeRepository.GetEmployee(1).Name;
            return employee;
        }
    }
}
