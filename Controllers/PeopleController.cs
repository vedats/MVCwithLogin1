using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MVCwithLogin1.Data;
using MVCwithLogin1.Models;

namespace MVCwithLogin1.Controllers
{
    public class PeopleController : Controller
    {
        private readonly HRDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public PeopleController(HRDBContext context , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await _context.Person.AsNoTracking().ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PerAd,Photo,ImageFile")] Person person)
        {
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/images
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(person.ImageFile.FileName);
                string extension = Path.GetExtension(person.ImageFile.FileName);
                person.Photo = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await person.ImageFile.CopyToAsync(fileStream);
                }
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PerAd,Photo")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Person.FindAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", person.Photo);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            //delete the record
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
       
        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}
