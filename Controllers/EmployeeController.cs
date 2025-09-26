using EmployeeManagement.Filters;
using EmployeeManagement.Models;
using EmployeeManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        public EmployeeController(ApplicationDbContext context) : base(context) { }

        public IActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Department)
                .Where(e => e.IsActive)
                .OrderBy(e => e.FirstName)
                .ToList();
            return View(employees);
        }

        [AdminOnly]
        public IActionResult Create()
        {
            var model = new EmployeeViewModel
            {
                Departments = new SelectList(db.Departments.Where(d => d.IsActive), "Id", "Name"),
                HireDate = DateTime.Today
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public IActionResult Create(EmployeeViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    model.Departments = new SelectList(db.Departments.Where(d => d.IsActive), "Id", "Name");
            //    return View(model);
            //}

            if (db.Employees.Any(e => e.Email == model.Email && e.IsActive))
            {
                ModelState.AddModelError("Email", "An employee with this email already exists.");
                model.Departments = new SelectList(db.Departments.Where(d => d.IsActive), "Id", "Name");
                return View(model);
            }

            if (!db.Departments.Any(d => d.Id == model.DepartmentId && d.IsActive))
            {
                ModelState.AddModelError("DepartmentId", "Selected department does not exist.");
                model.Departments = new SelectList(db.Departments.Where(d => d.IsActive), "Id", "Name");
                return View(model);
            }

            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Salary = model.Salary,
                HireDate = model.HireDate,
                DepartmentId = model.DepartmentId,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            db.Employees.Add(employee);
            db.SaveChanges();

            TempData["Success"] = "Employee created successfully!";
            return RedirectToAction("Index");
        }

        [Authorize]

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();

            var employee = db.Employees
                             .Include(e => e.Department)
                             .FirstOrDefault(e => e.Id == id && e.IsActive);

            if (employee == null) return NotFound();

            return View(employee);
        }

        [AdminOnly]
        public IActionResult Edit(int? id)
        {
            if (id == null) return BadRequest();

            var employee = db.Employees.Find(id);
            if (employee == null || !employee.IsActive) return NotFound();

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Phone = employee.Phone,
                Salary = employee.Salary,
                HireDate = employee.HireDate,
                DepartmentId = employee.DepartmentId,
                Departments = new SelectList(db.Departments.Where(d => d.IsActive), "Id", "Name", employee.DepartmentId)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public IActionResult Edit(EmployeeViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    model.Departments = new SelectList(db.Departments.Where(d => d.IsActive), "Id", "Name");
            //    return View(model);
            //}

            if (db.Employees.Any(e => e.Email == model.Email && e.Id != model.Id && e.IsActive))
            {
                ModelState.AddModelError("Email", "An employee with this email already exists.");
                model.Departments = new SelectList(db.Departments.Where(d => d.IsActive), "Id", "Name");
                return View(model);
            }

            if (!db.Departments.Any(d => d.Id == model.DepartmentId && d.IsActive))
            {
                ModelState.AddModelError("DepartmentId", "Selected department does not exist.");
                model.Departments = new SelectList(db.Departments.Where(d => d.IsActive), "Id", "Name");
                return View(model);
            }

            var employee = db.Employees.Find(model.Id);
            if (employee != null)
            {
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.Email = model.Email;
                employee.Phone = model.Phone;
                employee.Salary = model.Salary;
                employee.HireDate = model.HireDate;
                employee.DepartmentId = model.DepartmentId;

                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = "Employee updated successfully!";
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public IActionResult Delete(int id)
        {
            var employee = db.Employees.Find(id);
            if (employee != null)
            {
                employee.IsActive = false;
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Employee deleted successfully!";
            }
            return RedirectToAction("Index");
        }

        
        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckEmailExists(string email, int id)
        {
            var exists = db.Employees.Any(e => e.Email == email && e.IsActive && e.Id != id);
            return Json(!exists);
        }
    }
}
