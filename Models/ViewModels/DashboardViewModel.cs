// ViewModels/DashboardViewModel.cs
using System;
using System.Collections.Generic;

namespace Employee_Management_System.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalEmployees { get; set; }
        public int TotalDepartments { get; set; }
        public decimal AverageSalary { get; set; }
        public int NewHiresThisMonth { get; set; }
        public List<DepartmentStatistic> DepartmentStats { get; set; }
        public List<MonthlyHireData> MonthlyHires { get; set; }
    }

    public class DepartmentStatistic
    {
        public string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
        public decimal AverageSalary { get; set; }
    }

    public class MonthlyHireData
    {
        public string Month { get; set; }
        public int Count { get; set; }
    }
}
