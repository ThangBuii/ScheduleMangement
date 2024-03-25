using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_ASSIGNMENT.DTO;
using PRN221_ASSIGNMENT.Models;
using PRN221_ASSIGNMENT.Service;

namespace PRN221_ASSIGNMENT.Pages.Schedule
{
    public class ViewTimetableModel : PageModel
    {
        private ScheduleManagementContext _context;
        public ViewTimetableModel(ScheduleManagementContext context)
        {
            _context = context;
        }

        public List<WeekDTO> Weeks { get; set; }

        public List<DateTime> Days { get; set; }

        [BindProperty]
        public int WeekNumber { get; set; }

        [BindProperty]
        public int SelectedClass { get; set; }

        [BindProperty]
        public int SelectedTeacher { get; set; }

        public List<ScheduleDetail>[,] ScheduleList { get; set; }

        public List<Models.GroupClass> Classes { get; set; }
        public List<Models.Teacher> Teachers { get; set; }
        public void OnGet()
        {
            WeekNumber = 1;
            GetData();
        }

        private void GetData()
        {
            ScheduleDetailService scheduleDetailService = new ScheduleDetailService(_context);
            SlotService slotService = new SlotService(_context);

            ScheduleList = new List<ScheduleDetail>[12, 7];
            Weeks = slotService.FindWeekList();
            Classes = _context.GroupClasses.ToList();
            Teachers = _context.Teachers.ToList();
            
            var currentWeek = Weeks.FirstOrDefault(w => w.WeekNo == WeekNumber);
            Days = slotService.GetDaysBetween(currentWeek);
            List<ScheduleDetail> ScheduleInWeek = scheduleDetailService.FindScheduleDetailInWeek(currentWeek);

            if(SelectedClass != null && SelectedClass != 0)
            {
                ScheduleInWeek = ScheduleInWeek.Where(s => s.Schedule.ClassId == SelectedClass).ToList();
            }
            if (SelectedTeacher != null && SelectedTeacher != 0)
            {
                ScheduleInWeek = ScheduleInWeek.Where(s => s.Schedule.TeacherId == SelectedTeacher).ToList();
            }
            for (int i = 0; i < 12; i++)
            {
                var slotId = i + 1;
                for(int j = 0; j < 7; j++)
                {
                    DateTime date = slotService.FindDateBasedOnWeek(currentWeek, j);
                    ScheduleList[i,j] = ScheduleInWeek.Where(s => s.SlotId == slotId && s.Date == date).ToList();
                }
            }
        }

        public void OnPostFilter()
        {
            GetData();
        }

        public void OnPostClear()
        {
            WeekNumber = 1;
            SelectedClass = 0;
            SelectedTeacher = 0;
            GetData();
        }
    }
}
