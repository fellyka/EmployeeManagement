using EmployeeManagement.Models;

namespace EmployeeManagement.ViewModels
{
    public class HomeDetailsViewModels
    {
        /*We create a "ViewModel" hen a Model object does not contain all the data a view needs*/
        public Employee? Employee { get; set; }
        public string? PageTitle { get; set; }
    }
}
