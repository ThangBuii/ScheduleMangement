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
    public class IndexModel : PageModel
    {
        private readonly PRN221_ASSIGNMENT.Models.ScheduleManagementContext _context;

        public IndexModel(PRN221_ASSIGNMENT.Models.ScheduleManagementContext context)
        {
            _context = context;
        }

        public IList<Models.GroupClass> GroupClass { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.GroupClasses != null)
            {
                GroupClass = await _context.GroupClasses.ToListAsync();
            }
        }
    }
}
