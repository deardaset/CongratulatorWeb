using CongratulatorWeb.Data;
using CongratulatorWeb.Entities;
using CongratulatorWeb.Models;
using CongratulatorWeb.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CongratulatorWeb.Controllers
{
    public class PersonController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PersonController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Person
        public IActionResult Index()
        {
            var birthdays = _appDbContext.People.OrderBy(p => p.Name).ToList();
            return View(birthdays);
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
                _appDbContext.Add(person);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var person = await _appDbContext.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPersonRequest request)
        {
            if (id != request.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var person = await _appDbContext.People.FindAsync(id);
                    if (person == null)
                    {
                        return NotFound();
                    }
                    person.Name = request.Name;
                    person.BirthDate = request.BirthDate;
                    person.Relationship = request.Relationship;
                                        
                    await _appDbContext.SaveChangesAsync();
                }
                catch
                {
                    if (!BirthdayPersonExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _appDbContext.People
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _appDbContext.People.FindAsync(id);
            if (person != null)
            {
                if (!string.IsNullOrEmpty(person.PhotoPath))
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, person.PhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                _appDbContext.People.Remove(person);
                await _appDbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Birthday/UploadPhoto/5
        public IActionResult UploadPhoto(int id)
        {
            var model = new UploadPhotoViewModel { PersonId = id };
            return View(model);
        }

        // POST: Birthday/UploadPhoto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhoto(UploadPhotoViewModel model)
        {
            if (model.Photo != null && model.Photo.Length > 0)
            {
                var fileName = $"{model.PersonId}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(model.Photo.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                var person = await _appDbContext.People.FindAsync(model.PersonId);
                if (person != null)
                {
                    person.PhotoPath = $"/images/{fileName}";
                    await _appDbContext.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Edit), new { id = model.PersonId });
            }

            ModelState.AddModelError("", "Файл не выбран или пустой.");
            return View(model);
        }

        // Other method
        private bool BirthdayPersonExists(int id)
        {
            return _appDbContext.People.Any(e => e.Id == id);
        }
    }
}
