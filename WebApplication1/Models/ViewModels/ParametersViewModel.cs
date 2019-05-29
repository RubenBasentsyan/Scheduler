using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class ParametersViewModel
    {
        [Display(Name = "No. of Rooms")]
        [Required(ErrorMessage = "Please specify the number of rooms allocated to each time slot")]
        [Range(1, 100, ErrorMessage = "Number of rooms must be between 1 and 100")]
        public int ConcurrencyLimit { get; set; }

        [Display(Name = "No. of Days")]
        [Required(ErrorMessage = "Please specify the number of days")]
        [Range(1, 30, ErrorMessage = "Number of days must be between 1 and 30")]
        public int Days { get; set; }

        [Display(Name = "No. of Time Slots")]
        [Required(ErrorMessage = "Please specify the number of time slots")]
        [Range(1, 30, ErrorMessage = "Number of time slots must be between 1 and 30")]
        public int TimeSlots { get; set; }
    }
}