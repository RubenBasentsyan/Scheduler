using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class ParticipantsViewModel
    {
        [ScaffoldColumn(false), Display(AutoGenerateField = false)]
        public int PersonId { get; set; }

        [Display(Name = "Participant Name"), Required(ErrorMessage = "Enter the name of the course participant."),
         StringLength(45, ErrorMessage = "The name's length can't exceed 45 characters.")]
        public string Name { get; set; }
    }
}