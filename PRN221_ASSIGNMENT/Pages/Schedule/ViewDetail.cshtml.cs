using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_ASSIGNMENT.Models;

namespace PRN221_ASSIGNMENT.Pages.Schedule
{
    public class ViewDetailModel : PageModel
    {
        private ScheduleManagementContext _context;

        public ViewDetailModel(ScheduleManagementContext context)
        {
            _context = context;
        }
        public List<ScheduleDetail> ScheduleDetails { get; set; }
        public Models.Schedule Schedule;
        public void OnGet(int id)
        {
            GetData(id);
        }

        private void GetData(int id)
        {
            Schedule = _context.Schedules
                                    .Include(s => s.Class)
                                    .Include(s => s.Room)
                                        .ThenInclude(r => r.Building)
                                    .Include(s => s.Teacher)
                                    .Include(s => s.Subject)
                                    .Include(s => s.SlotTemplate).FirstOrDefault(s => s.Id == id);
            ScheduleDetails = _context.ScheduleDetails.Include(s => s.Slot).Where(s => s.ScheduleId == id).ToList();
        }
    }
}
