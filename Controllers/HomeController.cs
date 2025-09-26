using Employee_Management_System.Models;
using Employee_Management_System.Models.ViewModels;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Employee_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var model = new DashboardViewModel
            {
                TotalEmployees = _db.Employees.Count(e => e.IsActive),
                TotalDepartments = _db.Departments.Count(d => d.IsActive),
                AverageSalary = _db.Employees.Where(e => e.IsActive).Any()
                                ? _db.Employees.Where(e => e.IsActive).Average(e => e.Salary)
                                : 0,
                NewHiresThisMonth = _db.Employees.Count(e => e.IsActive &&
                                                        e.HireDate.Month == currentMonth &&
                                                        e.HireDate.Year == currentYear),

                DepartmentStats = _db.Departments.Where(d => d.IsActive)
                    .Select(d => new DepartmentStatistic
                    {
                        DepartmentName = d.Name,
                        EmployeeCount = d.Employees.Count(e => e.IsActive),
                        AverageSalary = d.Employees.Where(e => e.IsActive).Any()
                                        ? d.Employees.Where(e => e.IsActive).Average(e => e.Salary)
                                        : 0
                    }).ToList(),

                MonthlyHires = _db.Employees.Where(e => e.IsActive && e.HireDate.Year == currentYear)
                    .GroupBy(e => e.HireDate.Month)
                    .Select(g => new MonthlyHireData
                    {
                        Month = new DateTime(currentYear, g.Key, 1).ToString("MMMM"),
                        Count = g.Count()
                    }).ToList()
            };

            return View(model);
        }
    }
}
