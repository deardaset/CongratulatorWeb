using CongratulatorWeb.Data;
using CongratulatorWeb.Entities;
using CongratulatorWeb.Exceptions;
using CongratulatorWeb.Interfaces;
using CongratulatorWeb.Models;
using CongratulatorWeb.Models.Requests;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace CongratulatorWeb.Repositories
{
    public class PersonRepository(AppDbContext context, IWebHostEnvironment webhost) : IPersonRepository
    {
        public async Task CreatePersonAsync(Person person)
        {
            context.People.Add(person);
            await context.SaveChangesAsync();
        }
        public async Task<List<Person>> GetAllPeople()
        {
            var people = await context.People.ToListAsync();
            return people.OrderBy(p => p.Name).ToList();            
        }
        public async Task<Person> GetPersonByIdAsync(int id)
        {
            var person = await context.People.FirstOrDefaultAsync(p => p.Id == id);
            if (person == null) 
            {
                throw new PersonNotFoundException("Person not found");
            }
            return person;
        }
        public async Task DeletePersonAsync(Person person)
        {
            context.People.Remove(person);
            await context.SaveChangesAsync();
        }
        public async Task EditPersonAsync(int id, EditPersonRequest request)
        {
            var person = await context.People.FirstOrDefaultAsync(p => p.Id == id);
            if (person == null)
            {
                throw new PersonNotFoundException("Person not found");
            }
            person.Name = request.Name;
            person.BirthDate = request.BirthDate;
            person.Relationship = request.Relationship;

            await context.SaveChangesAsync();
        }
        public async Task UploadPhotoAsync(EditPhotoViewModel model)
        {
            var fileName = $"{model.PersonId}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(model.Photo.FileName)}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            var person = await context.People.FindAsync(model.PersonId);
            if (person != null)
            {
                person.PhotoPath = $"/images/{fileName}";
                await context.SaveChangesAsync();
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Photo.CopyToAsync(stream);
            }
        }
        public async Task DeletePhotoAsync(int id)
        {
            var person = await context.People.FirstOrDefaultAsync(p => p.Id == id);
            if (person == null)
            {
                throw new PersonNotFoundException("Person not found");
            }
            if (!string.IsNullOrEmpty(person.PhotoPath))
            {
                var path = Path.Combine(webhost.WebRootPath, person.PhotoPath.TrimStart('/'));
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            person.PhotoPath = null;
            await context.SaveChangesAsync();
        }
    }
}
