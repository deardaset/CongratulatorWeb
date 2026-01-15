using CongratulatorWeb.Models.Enums;

namespace CongratulatorWeb.Models.Requests
{
    public class EditPersonRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public RelationshipType Relationship { get; set; }
    }
}
