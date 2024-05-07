using LearningManagmentSystemWebApp.DAL.Models;

namespace LearningManagmentSystemWebApp.DAL
{
    public interface IEmployeeRepository
    {
        Task<IList<Employee>> GetAllAsync();
        IAsyncEnumerable<Employee> GetAllAsync2();
        Employee GetById(int id);
        int Insert (Employee employee);
        void Update (Employee employee);    
        void Delete (int id);
    }
}
