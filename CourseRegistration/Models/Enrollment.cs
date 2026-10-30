using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegistration.Models
{
    public class Enrollment
    {
        public long Id { get; set; }

        public int LearnerId { get; set; }

        public int CourseId { get; set; }

        public EnrollmentStatus Status { get; set; }
    }

    public enum EnrollmentStatus
    {
        Enrolled,
        Waitlisted,
        Dropped
    }
}
