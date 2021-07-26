using System;
using System.Collections.Generic;

namespace OnboardingCS.Models
{
    public class Grade
    {
        public Guid GradeId { get; set; }
        public string GradeName { get; set; }
        public string Section { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}