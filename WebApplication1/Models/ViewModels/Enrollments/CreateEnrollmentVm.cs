using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels.Enrollments
{
    public class CreateEnrollmentVm
    {  [Display(Name="Course name")]
        public int CourseId { get; set; }
        [Display(Name = "Participant name")]
        public int ParticipantId { get; set; }
        [ScaffoldColumn(false), Display(AutoGenerateField = false)]
        public IEnumerable<CourseViewModel> CoursesList { get; set; }
        [ScaffoldColumn(false), Display(AutoGenerateField = false)]
        public IEnumerable<ParticipantsViewModel> ParticipantList { get; set; }

    }
}