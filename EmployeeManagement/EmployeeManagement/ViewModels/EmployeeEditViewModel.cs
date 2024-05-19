namespace EmployeeManagement.ViewModels
{
    public class EmployeeEditViewModel : EmployeeCreateViewModel
    {
         
        public int id { get; set; }
        public string? ExistingPhotoPath { get; set; }
    }
}

