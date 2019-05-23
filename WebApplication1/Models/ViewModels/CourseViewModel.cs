using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class CourseViewModel
    {
        [Key]
        [ScaffoldColumn(false), Display(AutoGenerateField = false)]
        public int CourseId { get; set; }

        [Display(Name = "Course Name"), Required(ErrorMessage = "Enter the name of the course."),
         StringLength(45, ErrorMessage = "The name's length can't exceed 45 characters.")]
        public string Name { get; set; }
    }
}