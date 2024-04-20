using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeaveApplicationApp.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [StringLength(30, ErrorMessage = "30 character maximum")]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }
        [StringLength(30, ErrorMessage = "30 character maximum")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Birth date is required")]
        [Display(Name = "Birth date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Department ID is required")]
        [Display(Name = "Department ID")]
        [ForeignKey("Department")]
        public int FKDepartmentId { get; set; }

        // Navigation property to Department
        public virtual Department? Department { get; set; }
        //Navigation property to LeaveApplications
        public ICollection<LeaveApplication>? LeaveApplications { get; set; }
    }
}
