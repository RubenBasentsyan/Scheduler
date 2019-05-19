using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels.Enrollments
{
    public class DisplayEnrollmentVm
    {
        [ScaffoldColumn(false), Display(AutoGenerateField = false)]
        public int EnrollmentId { get; set; }
        [ScaffoldColumn(false), Display(AutoGenerateField = false)]
        public int PersonId { get; set; }
        [ScaffoldColumn(false), Display(AutoGenerateField = false)]
        public int CourseId { get; set; }
        [Display(Name = "Participant Name")]
        public string PersonName { get; set; }
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
    }
}