namespace EmployeeManagement.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);//Get aspecific employees

        IEnumerable<Employee> GetAllEmployees();//Get all emploees
    }
}
