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

        public List<Models.Teacher> Teachers {  get; set; }
        
        public List<Models.GroupClass> Classes { get; set; }

        public List<Models.SlotTemplate> Slots { get; set; }

        [BindProperty]
        public int SelectedTeacher {  get; set; }

        [BindProperty]
        public int SelectedClass { get; set; }

        [BindProperty]
        public int SelectedSlot { get; set; }

        [BindProperty]
        public int Page {  get; set; }

        public int TotalPages { get; set; }
        public void OnGet()
        {
            Page = 1;
            GetData();
        }

        private void GetData()
        {
            Classes = _context.GroupClasses.ToList();
            Teachers = _context.Teachers.ToList();
            Slots = _context.SlotTemplates.ToList();
            Schedules = _context.Schedules.Include(s => s.Class).Include(s => s.Room)
                                            .ThenInclude(r => r.Building).Include(s => s.Teacher)
                                            .Include(s => s.Subject).Include(s => s.SlotTemplate)
                                            .ToList();
            if(SelectedTeacher != 0 && SelectedTeacher != null)
            {
                Schedules = Schedules.Where(s => s.TeacherId == SelectedTeacher).ToList();
            }
            if (SelectedClass != 0 && SelectedClass != null)
            {
                Schedules = Schedules.Where(s => s.ClassId == SelectedClass).ToList();
            }
            if (SelectedSlot != 0 && SelectedSlot != null)
            {
                Schedules = Schedules.Where(s => s.SlotTemplateId == SelectedSlot).ToList();
            }

            var pageSize = 10;

            TotalPages = (int)Math.Ceiling(Schedules.Count() / (double)pageSize);
            Schedules = Schedules.Skip((Page - 1) * pageSize).Take(pageSize).ToList();
        }

        public void OnPostDelete()
        {
            ScheduleService service = new ScheduleService(_context);
            service.DeleteScheduleById(ScheduleId);
            GetData();
        }

        public void OnPostFilter()
        {
            GetData();
        }

        public void OnPostPaging()
        {
            GetData();
        }
    }
}
