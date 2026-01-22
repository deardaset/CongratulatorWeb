using CongratulatorWeb.Data;
using CongratulatorWeb.Entities;
using CongratulatorWeb.Exceptions;
using CongratulatorWeb.Interfaces;
using CongratulatorWeb.Models;
using CongratulatorWeb.Models.Requests;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CongratulatorWeb.Repositories
{
    public class PersonRepository(AppDbContext context, IWebHostEnvironment webhost) : IPersonRepository
    {
        public async Task CreatePersonAsync(Person person)
        {
            context.People.Add(person);
            await context.SaveChangesAsync();
        }
        public async Task<List<Person>> UpcomingBirthdaysAsync(DateTime today, string? sortBy = null)
        {
            const int ADD_DAYS = 30;

            var people = await context.People.ToListAsync();

            var sorted = sortBy?.ToLower() switch
            {
                "name" => people
                    .Where(p => p.NextBirthday >= today && p.NextBirthday <= today.AddDays(ADD_DAYS))
                    .OrderBy(p => p.Name),
                "year" => people
                    .Where(p => p.NextBirthday >= today && p.NextBirthday <= today.AddDays(ADD_DAYS))
                    .OrderBy(p => p.BirthDate.Year),
                "birthdate" => people
                    .Where(p => p.NextBirthday >= today && p.NextBirthday <= today.AddDays(ADD_DAYS))
                    .OrderBy(p => p.NextBirthday),
                "relationship" => people
                    .Where(p => p.NextBirthday >= today && p.NextBirthday <= today.AddDays(ADD_DAYS))
                    .OrderBy(p => p.Relationship),
                _ => people
                    .Where(p => p.NextBirthday >= today && p.NextBirthday <= today.AddDays(ADD_DAYS))
                    .OrderBy(p => p.NextBirthday)
            };

            return sorted.ToList();
        }
        public async Task<List<Person>> GetAllPeopleAsync(string? sortBy = null)
        {
            var people = await context.People.ToListAsync();

            var sorted = sortBy?.ToLower() switch
            {
                "name" => people.OrderBy(p => p.Name),
                "year" => people.OrderBy(p => p.BirthDate.Year),
                "birthdate" => people.OrderBy(p => p.NextBirthday),
                "relationship" => people.OrderBy(p => p.Relationship),
                _ => people.OrderBy(p => p.Name)
            };

            return sorted.ToList();
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
