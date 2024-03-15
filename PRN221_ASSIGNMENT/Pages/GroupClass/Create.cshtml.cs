using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN221_ASSIGNMENT.Models;

namespace PRN221_ASSIGNMENT.Pages.GroupClass
{
    public class CreateModel : PageModel
    {
        private readonly PRN221_ASSIGNMENT.Models.ScheduleManagementContext _context;

        public CreateModel(PRN221_ASSIGNMENT.Models.ScheduleManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.GroupClass GroupClass { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.GroupClasses == null || GroupClass == null)
            {
                return Page();
            }

            _context.GroupClasses.Add(GroupClass);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
