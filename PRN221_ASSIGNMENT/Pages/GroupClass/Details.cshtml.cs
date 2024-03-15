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
    public class DetailsModel : PageModel
    {
        private readonly PRN221_ASSIGNMENT.Models.ScheduleManagementContext _context;

        public DetailsModel(PRN221_ASSIGNMENT.Models.ScheduleManagementContext context)
        {
            _context = context;
        }

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
    }
}
