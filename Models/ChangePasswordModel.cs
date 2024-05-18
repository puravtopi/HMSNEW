using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class ChangePasswordModel:BaseResponseModel
    {
        public int Id { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "This field is required.")]
        public string Password { get; set; }

        [Display(Name = "NewPassowrd")]
        [Required(ErrorMessage = "This field is required.")]
        public string NewPassowrd { get; set; }

        [Display(Name = "ConfimPassword")]
        [Required(ErrorMessage = "This field is required.")]
        public string ConfimPassword { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
