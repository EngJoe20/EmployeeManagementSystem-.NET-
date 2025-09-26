using EmployeeManagement.Filters;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class DepartmentController : BaseController
    {
        public DepartmentController(ApplicationDbContext context) : base(context) { }

        public IActionResult Index()
        {
            var departments = db.Departments
                .Where(d => d.IsActive)
                .Include(d => d.Employees)
                .OrderBy(d => d.Name)
                .ToList();
            return View(departments);
        }
        [Authorize]
        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();

            var department = db.Departments
                .Include(d => d.Employees)
                .FirstOrDefault(d => d.Id == id && d.IsActive);

            if (department == null) return NotFound();

            return View(department);
        }


        [AdminOnly]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                if (db.Departments.Any(d => d.Name == department.Name && d.IsActive))
                {
                    ModelState.AddModelError("Name", "A department with this name already exists.");
                    return View(department);
                }
                department.IsActive = true;
                db.Departments.Add(department);
                db.SaveChanges();
                TempData["Success"] = "Department created successfully!";
                return RedirectToAction("Index");
            }
            return View(department);
        }

        [AdminOnly]
        public IActionResult Edit(int? id)
        {
            if (id == null) return BadRequest();
            var department = db.Departments.Find(id);
            if (department == null || !department.IsActive) return NotFound();
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public IActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                if (db.Departments.Any(d => d.Name == department.Name && d.Id != department.Id && d.IsActive))
                {
                    ModelState.AddModelError("Name", "A department with this name already exists.");
                    return View(department);
                }
                db.Entry(department).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Department updated successfully!";
                return RedirectToAction("Index");
            }
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public IActionResult Delete(int id)
        {
            var department = db.Departments.Find(id);
            if (department != null)
            {
                if (department.Employees.Any(e => e.IsActive))
                    TempData["Error"] = "Cannot delete department with active employees.";
                else
                {
                    department.IsActive = false;
                    db.Entry(department).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    TempData["Success"] = "Department deleted successfully!";
                }
            }
            return RedirectToAction("Index");
        }
    }
}
