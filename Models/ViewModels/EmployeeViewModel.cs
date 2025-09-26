using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Only letters and spaces allowed")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Only letters and spaces allowed")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Remote(action: "CheckEmailExists", controller: "Employee", AdditionalFields = nameof(Id), ErrorMessage = "This email is already in use.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Phone must be 10-15 digits, optional +")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(1, 1000000)]
        [Display(Name = "Monthly Salary")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Hire date is required")]
        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [NotMapped]
        public SelectList Departments { get; set; }
    }
}
