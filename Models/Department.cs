using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeaveApplicationApp.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }
        [StringLength(30, ErrorMessage = "30 character maximum")]
        [Required(ErrorMessage = "Department name required!")]
        [Display(Name = "Department Name")]
        public required string DepartmentName { get; set; }
        //Navigation property
        public ICollection<Employee>? Employees { get; set; }
    }
}
