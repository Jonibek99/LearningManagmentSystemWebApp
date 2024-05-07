namespace LearningManagmentSystemWebApp.DAL.Models
{
    public class Employee
    {
        public int? EmployeeId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string Title { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
