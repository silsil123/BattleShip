using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Games
{
    public class EditModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public EditModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Game? Game { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Game = await _context.Games
                .Include(g => g.FirstPlayer)
                .Include(g => g.SecondPlayer).FirstOrDefaultAsync(m => m.GameId == id);

            if (Game == null)
            {
                return NotFound();
            }
           ViewData["FirstPlayerId"] = new SelectList(_context.Players, "PlayerId", "Name");
           ViewData["SecondPlayerId"] = new SelectList(_context.Players, "PlayerId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(Game!.GameId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }
    }
}
