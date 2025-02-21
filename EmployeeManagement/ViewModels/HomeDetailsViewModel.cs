using EmployeeManagement.Models;

namespace EmployeeManagement.ViewModels
{
    /*This ViewModel class is a DTO*/
    public class HomeDetailsViewModel
    {
        public Employee Employee { get; set; } = new Employee();
        public string PageTitle { get; set; } = String.Empty;
    }
}
