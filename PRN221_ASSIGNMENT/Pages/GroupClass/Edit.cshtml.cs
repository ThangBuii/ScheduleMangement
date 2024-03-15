using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN221_ASSIGNMENT.Models;

namespace PRN221_ASSIGNMENT.Pages.GroupClass
{
    public class EditModel : PageModel
    {
        private readonly PRN221_ASSIGNMENT.Models.ScheduleManagementContext _context;

        public EditModel(PRN221_ASSIGNMENT.Models.ScheduleManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.GroupClass GroupClass { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.GroupClasses == null)
            {
                return NotFound();
            }

            var groupclass =  await _context.GroupClasses.FirstOrDefaultAsync(m => m.Id == id);
            if (groupclass == null)
            {
                return NotFound();
            }
            GroupClass = groupclass;
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

            _context.Attach(GroupClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupClassExists(GroupClass.Id))
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

        private bool GroupClassExists(int id)
        {
          return (_context.GroupClasses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
