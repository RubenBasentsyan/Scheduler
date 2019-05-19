using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels.Enrollments
{
    public class CreateEnrollmentVm
    {
        [ScaffoldColumn(false), Display(AutoGenerateField = false)]
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public int ParticipantId { get; set; }
        [ScaffoldColumn(false), Display(AutoGenerateField = false)]
        public List<CourseViewModel> CoursesList { get; set; }
        [ScaffoldColumn(false), Display(AutoGenerateField = false)]
        public List<ParticipantsViewModel> ParticipantList { get; set; }

    }
}