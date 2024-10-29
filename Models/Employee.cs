using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; } // Primärnyckel

        [Required]
        public string Name { get; set; } // Anställdens namn

        [Phone] // Validering för telefonnummer
        public string PhoneNumber { get; set; } // Anställdens telefonnummer

        [EmailAddress] // Validering för e-postadress
        public string Email { get; set; } // Anställdens e-postadress

        // Initiera samlingen för att undvika null-referenser
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}