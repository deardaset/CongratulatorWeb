using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CongratulatorWeb.Models
{
    public class UploadPhotoViewModel
    {
        [Required(ErrorMessage = "Выберите файл")]
        [Display(Name = "Фотография")]
        public IFormFile? Photo { get; set; }

        public int PersonId { get; set; }
    }
}
