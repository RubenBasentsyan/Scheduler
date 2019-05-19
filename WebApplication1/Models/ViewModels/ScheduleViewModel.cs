using System.ComponentModel.DataAnnotations;
using WebApplication1.Helpers;

namespace WebApplication1.Models.ViewModels
{
    public class ScheduleViewModel
    {
        [ScaffoldColumn(false), Display(AutoGenerateField = false)]
        public int CourseId { get; set; }
        [Display(Name = "Course Name")]
        public string Course { get; set; }
        [Display(Name = "Day")]
        public string Day { get; set; }
        [Display(Name = "Time Slot")]
        public string TimeSlot { get; set; }
    }
}