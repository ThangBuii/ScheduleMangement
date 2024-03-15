using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_ASSIGNMENT.Models;

namespace PRN221_ASSIGNMENT.Pages.GroupClass
{
    public class DeleteModel : PageModel
    {
        private readonly PRN221_ASSIGNMENT.Models.ScheduleManagementContext _context;

        public DeleteModel(PRN221_ASSIGNMENT.Models.ScheduleManagementContext context)
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

            var groupclass = await _context.GroupClasses.FirstOrDefaultAsync(m => m.Id == id);

            if (groupclass == null)
            {
                return NotFound();
            }
            else 
            {
                GroupClass = groupclass;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.GroupClasses == null)
            {
                return NotFound();
            }
            var groupclass = await _context.GroupClasses.FindAsync(id);

            if (groupclass != null)
            {
                GroupClass = groupclass;
                _context.GroupClasses.Remove(GroupClass);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
