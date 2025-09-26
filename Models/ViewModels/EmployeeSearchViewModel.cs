// ViewModels/EmployeeSearchViewModel.cs
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class EmployeeSearchViewModel
{
    public string SearchTerm { get; set; }
    public int? DepartmentId { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public DateTime? HireDateFrom { get; set; }
    public DateTime? HireDateTo { get; set; }

    public IEnumerable<Employee> Employees { get; set; }
    public SelectList Departments { get; set; }
}