using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem
{
    public class LeaveRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        [DataType(DataType.Date)] 
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [ForeignKey("LeaveType")]
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }

        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [NotMapped]
        public int TotalDays => (EndDate - StartDate).Days + 1;
    }

}
