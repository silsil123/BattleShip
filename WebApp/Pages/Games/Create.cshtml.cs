using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace WebApp.Pages_Games
{
    public class CreateModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public CreateModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["FirstPlayerId"] = new SelectList(_context.Players, "PlayerId", "Name");
        ViewData["SecondPlayerId"] = new SelectList(_context.Players, "PlayerId", "Name");
            return Page();
        }

        [BindProperty] 
        public Game? Game { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Games.Add(Game!);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
