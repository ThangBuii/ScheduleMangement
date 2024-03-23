using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using PRN221_ASSIGNMENT.DTO;
using PRN221_ASSIGNMENT.Mapper;
using PRN221_ASSIGNMENT.Models;
using PRN221_ASSIGNMENT.Service;
using System.Drawing.Text;

namespace PRN221_ASSIGNMENT.Pages.Schedule
{
    public class EditModel : PageModel
    {
        private ScheduleManagementContext _context;



        public EditModel(ScheduleManagementContext context)
        {
            _context = context;
        }
        [BindProperty]
        public CsvData FormData { get; set; }

        public List<RoomDTO> Rooms { get; set; }
        public List<Models.Subject> Subjects { get; set; }
        public List<Models.Teacher> Teachers { get; set; }
        public List<Models.GroupClass> Classes { get; set; }

        public List<Models.SlotTemplate> Slots { get; set; }

        [BindProperty]
        public int ScheduleId { get; set; }
        public void OnGet(int id)
        {
            GetData(id);
        }

        public void GetData(int id)
        {
            ScheduleId = id;
            SlotService slotService = new SlotService(_context);
            Models.Schedule schedule = _context.Schedules
                .Include(s => s.Class).Include(s => s.Room).ThenInclude(r => r.Building).Include(s => s.Teacher).Include(s => s.Subject).Include(s => s.SlotTemplate).FirstOrDefault(s => s.Id == id);
            FormData = new CsvData
            {
                Room = $"{schedule.Room.Building.Code}-{schedule.Room.Code}",
                Teacher = schedule.Teacher.Code,
                Class = schedule.Class.Code,
                Subject = schedule.Subject.Code,
                TimeSlot = schedule.SlotTemplate.Code

            };
            RoomMapper mapper = new RoomMapper(_context);
            Rooms = mapper.ToRoomDTO();
            Subjects = _context.Subjects.ToList();
            Teachers = _context.Teachers.ToList();
            Classes = _context.GroupClasses.ToList();
            Slots = _context.SlotTemplates.ToList();
        }

        public void OnPost()
        {
            DataService service = new DataService(_context);

            string message = service.EditData(FormData,ScheduleId);
            if (message == "Saved successfully!")
            {
                // Redirect to another page
                Response.Redirect("/Schedule/View");
            }

            // If adding data failed, set ViewData and stay on the current page
            ViewData["Messages"] = message;
            GetData(ScheduleId);
        }
    }
}
