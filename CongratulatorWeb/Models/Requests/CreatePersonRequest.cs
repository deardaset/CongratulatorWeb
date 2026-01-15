using CongratulatorWeb.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CongratulatorWeb.Models.Requests
{
    public class CreatePersonRequest
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public RelationshipType Relationship { get; set; }
    }
}
