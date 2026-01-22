using CongratulatorWeb.Entities;
using CongratulatorWeb.Models;
using CongratulatorWeb.Models.Requests;

namespace CongratulatorWeb.Interfaces
{
    public interface IPersonRepository
    {
        public Task CreatePersonAsync(Person person);
        public Task<List<Person>> UpcomingBirthdaysAsync(DateTime today, string? sortBy = null);
        public Task<List<Person>> GetAllPeopleAsync(string? sortBy = null);
        public Task<Person> GetPersonByIdAsync(int id);
        public Task DeletePersonAsync(Person person);
        public Task EditPersonAsync(int id, EditPersonRequest request);
        public Task UploadPhotoAsync(EditPhotoViewModel model);
        public Task DeletePhotoAsync(int id);

    }
}
