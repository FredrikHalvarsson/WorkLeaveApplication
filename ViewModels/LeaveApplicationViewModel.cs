using LeaveApplicationApp.Models;

namespace LeaveApplicationApp.ViewModels
{
    public class LeaveApplicationViewModel
    {
        public string EmployeeName { get; set; }
        public DateTime ApplicationTime { get; set; }
        public int TotalDaysApplied { get; set; }
        public DateTime StartDate { get; set; }
        public LeaveType WorkLeaveType { get; set; }
    }
}
