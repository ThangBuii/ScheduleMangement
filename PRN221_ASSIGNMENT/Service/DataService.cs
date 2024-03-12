
using PRN221_ASSIGNMENT.DTO;
using PRN221_ASSIGNMENT.Models;

namespace PRN221_ASSIGNMENT.Service
{
    public class DataService
    {
        private ScheduleManagementContext _context;
        public DataService(ScheduleManagementContext context)
        {
            _context = context;
        }

        private ScheduleDTO GetFullData(CsvData data)
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
            
            Schedule1.ClassId = _context.Classes.FirstOrDefault(c => c.Code == data.Class).Id;
            Schedule2.ClassId = Schedule1.ClassId;
            
            Schedule1.SlotId = 0;
            Schedule1.Date = DateTime.Now;
            return scheduleDTO;
            
        }

        public bool ValidateData(CsvData data)
        {

            return false;
        }

        public void AddDataToDatabase(CsvData data)
        {
            
        }
    }
}
