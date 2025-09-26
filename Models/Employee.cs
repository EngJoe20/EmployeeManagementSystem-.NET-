using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Salary must be greater than 0")]
        [Display(Name = "Monthly Salary")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Hire date is required")]
        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
