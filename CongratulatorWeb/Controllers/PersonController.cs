using CongratulatorWeb.Data;
using CongratulatorWeb.Entities;
using CongratulatorWeb.Exceptions;
using CongratulatorWeb.Interfaces;
using CongratulatorWeb.Models;
using CongratulatorWeb.Models.Requests;
using CongratulatorWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading.Tasks;

namespace CongratulatorWeb.Controllers
{
    public class PersonController (IPersonRepository repository, IWebHostEnvironment webhost) : Controller
    {
        // GET: Person
        public async Task<IActionResult> Index(string? sortBy)
        {
            var people = await repository.GetAllPeopleAsync(sortBy);
            ViewBag.SortBy = sortBy;
            return View(people);
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePersonRequest request)
        {
            if (ModelState.IsValid)
            {
                var person = new Person
                {
                    Name = request.Name,
                    BirthDate = request.BirthDate,
                    Relationship = request.Relationship
                };
                await repository.CreatePersonAsync(person);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }
        
        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var person = await repository.GetPersonByIdAsync(id);
            return View(person);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPersonRequest request)
        {
            if (id != request.Id)
                throw new PersonNotFoundException("Person not found");
            if (ModelState.IsValid)
            {
                await repository.EditPersonAsync(id, request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var person = await repository.GetPersonByIdAsync(id);            
            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await repository.GetPersonByIdAsync(id);
            if (person != null)
            {
                if (!string.IsNullOrEmpty(person.PhotoPath))
                {
                    var path = Path.Combine(webhost.WebRootPath, person.PhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                await repository.DeletePersonAsync(person);                
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Birthday/UploadPhoto/5
        public async Task<IActionResult> EditPhoto(int id)
        {
            var person = await repository.GetPersonByIdAsync(id);
            if (person == null)
            {
                throw new PersonNotFoundException("Person not found");
            }
            var model = new EditPhotoViewModel
            {
                PersonId = person.Id,
                Name = person.Name,
                CurrentPhotoPath = person.PhotoPath
            };

            return View(model);
        }

        // POST: Birthday/UploadPhoto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPhoto(EditPhotoViewModel model)
        {
            if (model.Photo != null && model.Photo.Length > 0)
            {
                await repository.UploadPhotoAsync(model);

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Файл не выбран или пустой.");
            return View(model);
        }

        public async Task<IActionResult> DeletePhoto(int id)
        {
            await repository.DeletePhotoAsync(id);
            return RedirectToAction(nameof(EditPhoto), new {id});
        }
    }
}
