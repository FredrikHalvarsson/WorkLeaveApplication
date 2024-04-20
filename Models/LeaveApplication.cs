using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static LeaveApplicationApp.Extentions.DateRangeAttributeExtention;

namespace LeaveApplicationApp.Models
{
    public enum LeaveType
    {
        Holliday,
        Parental,
        Sick,
        [Display(Name = "Care Of Child")]
        CareOfChild
    }
    public enum Status
    {
        Pending,
        Approved,
        Denied
    }
    public class LeaveApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveApplicationId { get; set; }
        public Status Status { get; set; } = Status.Pending;
        [Required]
        [Display(Name = "Leave Type")]
        public required LeaveType WorkLeaveType { get; set; }
        [Display(Name = "Application Submition Date")]
        public DateTime ApplicationTime { get; set; } = DateTime.Now;
        [Required]
        [DataType(DataType.Date)]
        [DateCorrectRange(ValidateStartDate = true, ErrorMessage = "Start date can't be older than the current date")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DateCorrectRange(ValidateEndDate = true, ErrorMessage = "End date can't be before the start date")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Employee")]
        [ForeignKey("Employee")]
        public int FkEmployeeId { get; set; }

        // Navigation property to Department
        public virtual Employee? Employee { get; set; }
    }
}
