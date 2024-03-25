using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_ASSIGNMENT.DTO;
using PRN221_ASSIGNMENT.Models;
using PRN221_ASSIGNMENT.Service;

namespace PRN221_ASSIGNMENT.Pages.Schedule
{
    public class ViewScheduleDetailModel : PageModel
    {
        private ScheduleManagementContext _context;

        public ViewScheduleDetailModel(ScheduleManagementContext context)
        {
            _context = context;
        }

        public CsvData ScheduleData { get; set; }

        [BindProperty]
        public ScheduleDetail ScheduleDetail { get; set; }

        [BindProperty]
        public int ScheduleDetailId { get; set; }

        public List<Slot> SlotList { get; set; }

        public List<DateTime> DateTimes { get; set; }

        public void OnGet(int id)
        {
            GetData(id);
        }

        private void GetData(int id)
        {
            SlotService slotService = new SlotService(_context);
            ScheduleDetailId = id;
            DateTimes = slotService.FindDateTimeList();
            ScheduleDetail = _context.ScheduleDetails.Include(s => s.Slot).FirstOrDefault(s => s.Id == id);
            Models.Schedule schedule = _context.Schedules
               .Include(s => s.Class).Include(s => s.Room).ThenInclude(r => r.Building).Include(s => s.Teacher).Include(s => s.Subject).Include(s => s.SlotTemplate).FirstOrDefault(s => s.Id == ScheduleDetail.ScheduleId);
            ScheduleData = new CsvData
            {
                Room = $"{schedule.Room.Building.Code}-{schedule.Room.Code}",
                Teacher = schedule.Teacher.Code,
                Class = schedule.Class.Code,
                Subject = schedule.Subject.Code,
                TimeSlot = schedule.SlotTemplate.Code

            };
            SlotList = _context.Slots.ToList();
        }
    }
}
