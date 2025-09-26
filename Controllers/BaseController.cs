using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ApplicationDbContext db;

        public BaseController(ApplicationDbContext context)
        {
            db = context;
        }

        protected bool IsAdmin => User.IsInRole("Admin");
        protected bool IsUser => User.IsInRole("User");
    }
}
