using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_ASSIGNMENT.Models;
using PRN221_ASSIGNMENT.Service;

namespace PRN221_ASSIGNMENT.Pages.Schedule
{
    public class ViewModel : PageModel
    {
        private readonly ScheduleManagementContext _context;

        public ViewModel(ScheduleManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int ScheduleId { get; set; }

        public List<Models.Schedule> Schedules { get; set; }
        public void OnGet()
        {
            GetData();
        }

        private void GetData()
        {
            Schedules = _context.Schedules.Include(s => s.Class).Include(s => s.Room)
                                            .ThenInclude(r => r.Building).Include(s => s.Teacher)
                                            .Include(s => s.Subject).Include(s => s.SlotTemplate)
                                            .ToList();
        }

        public void OnPost()
        {
            ScheduleService service = new ScheduleService(_context);
            service.DeleteScheduleById(ScheduleId);
            GetData();
        }
    }
}
