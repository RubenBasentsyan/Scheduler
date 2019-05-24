using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebApplication1.Models.ViewModels
{
    public class ParticipantsViewModel
    {
        [ScaffoldColumn(false), Display(AutoGenerateField = false)]
        public int PersonId { get; set; }

        [Display(Name = "Participant Name"), Required(ErrorMessage = "Enter the name of the course participant."),
         StringLength(45, ErrorMessage = "The name's length can't exceed 45 characters.")]
        public string Name { get; set; }

        [Display(Name = "Username"), Required(ErrorMessage = "Enter the username.")]
        public string Username { get; set; }

        [Display(Name = "Password"), Required(ErrorMessage = "Enter the password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}