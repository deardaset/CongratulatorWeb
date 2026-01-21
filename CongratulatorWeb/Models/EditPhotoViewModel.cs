using CongratulatorWeb.Exceptions;
using CongratulatorWeb.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace CongratulatorWeb.Models
{
    public class EditPhotoViewModel
    {
        [Required(ErrorMessage = "Выберите файл")]
        public IFormFile? Photo { get; set; }
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string? CurrentPhotoPath { get; set; }
    }
}
