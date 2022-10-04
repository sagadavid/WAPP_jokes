using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jokesWapp.Data;
using jokesWapp.Models;
using Microsoft.AspNetCore.Authorization;

namespace jokesWapp.Controllers
{
    public class jokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public jokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: jokes
        public async Task<IActionResult> Index()
        {
              return View(await _context.joke.ToListAsync());
        }

        // GET: jokes/showsearchform
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: jokes/showsearchresult
        //public string ShowSearchResult(string SøkeOrd)
        public async Task<IActionResult> ShowSearchResult(string SøkeOrd)
        {
            //return "du søker "+SøkeOrd;
            return View("Index",await _context.joke.Where(j=>j.question.Contains(SøkeOrd)).ToListAsync());
        }

        // GET: jokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.joke == null)
            {
                return NotFound();
            }

            var joke = await _context.joke
                .FirstOrDefaultAsync(m => m.id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // GET: jokes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: jokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,question,answer")] joke joke)
        {
            if (ModelState.IsValid)
            {
                _context.Add(joke);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }

        // GET: jokes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.joke == null)
            {
                return NotFound();
            }

            var joke = await _context.joke.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }
            return View(joke);
        }

        // POST: jokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,question,answer")] joke joke)
        {
            if (id != joke.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(joke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!jokeExists(joke.id))
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
            return View(joke);
        }

        // GET: jokes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.joke == null)
            {
                return NotFound();
            }

            var joke = await _context.joke
                .FirstOrDefaultAsync(m => m.id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // POST: jokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.joke == null)
            {
                return Problem("Entity set 'ApplicationDbContext.joke'  is null.");
            }
            var joke = await _context.joke.FindAsync(id);
            if (joke != null)
            {
                _context.joke.Remove(joke);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool jokeExists(int id)
        {
          return _context.joke.Any(e => e.id == id);
        }
    }
}
