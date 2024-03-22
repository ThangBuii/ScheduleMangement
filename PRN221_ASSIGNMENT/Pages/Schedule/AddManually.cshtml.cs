using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_ASSIGNMENT.DTO;
using PRN221_ASSIGNMENT.Mapper;
using PRN221_ASSIGNMENT.Models;
using PRN221_ASSIGNMENT.Service;

namespace PRN221_ASSIGNMENT.Pages.Schedule
{
    public class AddManuallyModel : PageModel
    {
        private readonly ScheduleManagementContext _context;
        [BindProperty]
        public CsvData FormData { get; set; }

        public List<RoomDTO> Rooms { get; set; }
        public List<Models.Subject> Subjects { get; set; }
        public List<Models.Teacher> Teachers { get; set; }
        public List<Models.GroupClass> Classes { get; set; }

        public List<Models.SlotTemplate> Slots { get; set; }


        public AddManuallyModel(ScheduleManagementContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            GetData();
        }

        public void GetData()
        {
            RoomMapper mapper = new RoomMapper(_context);
            Rooms = mapper.ToRoomDTO();
            Subjects = _context.Subjects.ToList();
            Teachers = _context.Teachers.ToList();
            Classes = _context.GroupClasses.ToList();
            Slots = _context.SlotTemplates.ToList();
        }

        public void OnPost() 
        {
            GetData();
            DataService service = new DataService(_context);
            
            string message = service.AddDataToDatabase(FormData);
            ViewData["Messages"] = message;
        }
    }
}
