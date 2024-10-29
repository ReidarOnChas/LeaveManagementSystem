using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem
{
    public class LeaveType
    {
        [Key] // Anger att detta är primärnyckeln
        public int TypeId { get; set; } // Primärnyckel

        [Required] // Validering för att säkerställa att ledighetstypens namn alltid är ifyllt
        public string LeaveName { get; set; } // Namn på ledighetstypen (t.ex. semester, sjukledighet)

        // Samling av relaterade ledighetsansökningar
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}
