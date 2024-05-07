using LearningManagmentSystemWebApp.DAL;
using LearningManagmentSystemWebApp.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagmentSystemWebApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<IActionResult> Index()
        {
            var list  = await _employeeRepository.GetAllAsync();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            return View(_employeeRepository.GetById(id));

        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            try
            {
                
               int id = _employeeRepository.Insert(employee);
                    return RedirectToAction("Details", new {id = id});
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            return View(_employeeRepository.GetById(id));
        }
        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            try
            {

                _employeeRepository.Update(employee);
                return RedirectToAction("Details", new { id = employee.EmployeeId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        public IActionResult Delete (int id)
        {
           try
            {
                _employeeRepository.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
