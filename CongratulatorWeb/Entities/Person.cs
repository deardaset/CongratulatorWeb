using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CongratulatorWeb.Models.Enums;

namespace CongratulatorWeb.Entities
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 chars")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Birthday is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Relationship is required")]
        public RelationshipType Relationship {  get; set; }

        [Display(Name = "Photo")]
        public string? PhotoPath { get; set; }

        [NotMapped]
        public int Age => CalculateAge(BirthDate);
        
        private int CalculateAge(DateTime birtDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birtDate.Year;
            if (birtDate > today.AddYears(-age)) age--;
            return age;
        }

        [NotMapped]
        public DateTime NextBirthday
        {
            get
            {
                var today = DateTime.Today;
                var next = new DateTime(today.Year, BirthDate.Month, BirthDate.Day);
                if (next < today) next = next.AddYears(1);
                return next;
            }
        }
    }
}
