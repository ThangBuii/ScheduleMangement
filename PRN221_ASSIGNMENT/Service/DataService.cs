
using PRN221_ASSIGNMENT.DTO;
using PRN221_ASSIGNMENT.Models;

namespace PRN221_ASSIGNMENT.Service
{
    public class DataService
    {
        private ScheduleManagementContext _context;

        private SlotService slotService;
        public DataService(ScheduleManagementContext context)
        {
            _context = context;
            slotService = new SlotService(_context);
        }

        private ScheduleDTO GetInitialData(CsvData data)
        {
            ScheduleDTO scheduleDTO = new ScheduleDTO();

            Schedule Schedule1 = new Schedule();
            Schedule Schedule2 = new Schedule();

            Schedule1.TeacherId = _context.Teachers.FirstOrDefault(t => t.Code == data.Teacher).Id;
            Schedule2.TeacherId = Schedule1.TeacherId;

            Schedule1.RoomId = _context.Rooms.FirstOrDefault(r => r.Code == data.Room).Id;
            Schedule2.RoomId = Schedule1.RoomId;
            
            Schedule1.SubjectId = _context.Subjects.FirstOrDefault(s => s.Code == data.Subject).Id;
            Schedule2.SubjectId = Schedule1.SubjectId;
            
            Schedule1.ClassId = _context.GroupClasses.FirstOrDefault(c => c.Code == data.Class).Id;
            Schedule2.ClassId = Schedule1.ClassId;

            if (data.TimeSlot[0] == 'A')
            {
                Schedule1.SlotId = 1;
                Schedule2.SlotId = 2;
            }
            else if (data.TimeSlot[0] == 'P')
            {
                Schedule1.SlotId = 3;
                Schedule2.SlotId = 4;
            }

            Schedule1.Date = slotService.FindInitialDate(data.TimeSlot[1]);
            Schedule2.Date = slotService.FindInitialDate(data.TimeSlot[2]);
            
            return scheduleDTO;
        }

        public List<Schedule> GetListSchedule(ScheduleDTO scheduleDTO)
        {
            List<Schedule> schedules = new List<Schedule>();
            Schedule schedule1 = scheduleDTO.Schedule1;
            schedules.Add(schedule1);
            Schedule schedule2 = scheduleDTO.Schedule2;
            schedules.Add(schedule2);
            for (int i = 1; i <= 20; i++)
            {
                Schedule newSchedule1 = schedule1;
                newSchedule1.Date = slotService.FindNextDate(i, schedule1.Date);
                schedules.Add(newSchedule1);
                Schedule newSchedule2 = schedule2;
                newSchedule2.Date = slotService.FindNextDate(i, schedule2.Date);
                schedules.Add(newSchedule1);
            }

            return schedules;
        }

        public bool IsExist(CsvData data)
        {
            if (_context.Teachers.FirstOrDefault(t => t.Code == data.Teacher) == null) return false;
            if (_context.Subjects.FirstOrDefault(t => t.Code == data.Subject) == null) return false;
            if (_context.GroupClasses.FirstOrDefault(t => t.Code == data.Class) == null) return false;
            if (_context.Rooms.FirstOrDefault(t => t.Code == data.Room) == null) return false;
            if(slotService.CheckExists(data.TimeSlot)) return false;

            return true;
        }

        public bool ValidateData(CsvData data)
        {
            if (IsExist(data))
            {

            }
            return false;
        }

        public void SaveToDb(List<Schedule> schedules) 
        {
            foreach(Schedule s in schedules)
            {
                _context.Add(s);
            }
            _context.SaveChanges();
        }

        public void AddDataToDatabase(CsvData data)
        {
            ScheduleDTO scheduleDTO = GetInitialData(data);
            List<Schedule> schedules = GetListSchedule(scheduleDTO);


            SaveToDb(schedules);
        }
    }
}
