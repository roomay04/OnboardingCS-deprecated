using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingCS.Models
{
    public class Student
    {
        public Guid StudentID { get; set; }
        public String StudentName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte[] Photo { get; set; }
        public decimal Height { get; set; }
        public float Weight { get; set; }
        public Grade grade { get; set; }
        public ICollection<Course> course { get; set; }
    }
}
