
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
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
        private ScheduleDTO GetInitialData(CsvData data,int id)
        {

            ScheduleDetail Schedule1 = new ScheduleDetail();
            ScheduleDetail Schedule2 = new ScheduleDetail();

            Schedule1.ScheduleId = id;
            Schedule2.ScheduleId = id;

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

            return new ScheduleDTO
            {
                Schedule1 = Schedule1,
                Schedule2 = Schedule2,
            };
        }

        public List<ScheduleDetail> GetListSchedule(ScheduleDTO scheduleDTO)
        {
            List<ScheduleDetail> schedules = new List<ScheduleDetail>();
            ScheduleDetail schedule1 = scheduleDTO.Schedule1;
            schedules.Add(schedule1);
            ScheduleDetail schedule2 = scheduleDTO.Schedule2;
            schedules.Add(schedule2);

            for (int i = 1; i < 10; i++)
            {
                ScheduleDetail newSchedule1 = copySchedule(schedule1);
                newSchedule1.Date = slotService.FindNextDate(i, (DateTime)schedule1.Date);
                schedules.Add(newSchedule1);
                ScheduleDetail newSchedule2 = copySchedule(schedule2);
                newSchedule2.Date = slotService.FindNextDate(i, (DateTime)schedule2.Date);
                schedules.Add(newSchedule2);
            }

            return schedules;
        }

        private ScheduleDetail copySchedule(ScheduleDetail initial)
        {
            return new ScheduleDetail
            {
                ScheduleId = initial.ScheduleId,
                SlotId = initial.SlotId,
                Date = initial.Date,
            };
        }

        public int IsExist(CsvData data)
        {
            string[] roomParts = data.Room.Split('-');
            string buildingCode = roomParts[0];
            string roomCode = roomParts[1];
            if (_context.Teachers.FirstOrDefault(t => t.Code == data.Teacher) == null) return 1;
            if (_context.Subjects.FirstOrDefault(t => t.Code == data.Subject) == null) return 2;
            if (_context.GroupClasses.FirstOrDefault(t => t.Code == data.Class) == null) return 3;
            if (_context.Buildings.FirstOrDefault(t => t.Code == buildingCode) == null) return 4;
            if (_context.Rooms.Include(t => t.Building).FirstOrDefault(t => t.Building.Code == buildingCode
            && t.Code == roomCode) == null) return 4;
            if (_context.SlotTemplates.FirstOrDefault(s => s.Code == data.TimeSlot)==null) return 5;

            return 0;
        }

        public void SaveToDb(List<ScheduleDetail> schedules)
        {
            foreach (ScheduleDetail s in schedules)
            {
                _context.ScheduleDetails.Add(s);
            }
            _context.SaveChanges();
        }

        public string AddDataToDatabase(CsvData data)
        {
            int messageId = IsExist(data);
            if (messageId == 0)
            {
                Schedule schedule = GetScheduleFromData(data);
                
                string message = IsValid(schedule);
                if (message != "")
                {
                    return message;
                }
                _context.Schedules.Add(schedule);
                _context.SaveChanges();

                ScheduleDTO scheduleDTO = GetInitialData(data,schedule.Id);
                List<ScheduleDetail> schedules = GetListSchedule(scheduleDTO);

                SaveToDb(schedules);
            }
            else
            {
                if (messageId == 1)
                {
                    return "Teacher code do not exists!";
                }
                else if (messageId == 2)
                {
                    return "Subject code do not exists!";
                }
                else if (messageId == 3)
                {
                    return "Class code do not exists!";
                }
                else if (messageId == 4)
                {
                    return "Room code do not exists!";
                }
                else if (messageId == 5)
                {
                    return "Slot do not exists!";
                }
            }


            return "Saved successfully!";
        }

        private Schedule GetScheduleFromData(CsvData data)
        {
            string[] roomParts = data.Room.Split('-');
            string buildingCode = roomParts[0];
            string roomCode = roomParts[1];
            Schedule schedule = new Schedule();
            schedule.TeacherId = _context.Teachers.FirstOrDefault(t => t.Code == data.Teacher).Id;

            schedule.RoomId = _context.Rooms.Include(r => r.Building)
                                             .FirstOrDefault(r => r.Building.Code == buildingCode && r.Code == roomCode).Id;

            schedule.SubjectId = _context.Subjects.FirstOrDefault(s => s.Code == data.Subject).Id;

            schedule.ClassId = _context.GroupClasses.FirstOrDefault(c => c.Code == data.Class).Id;
            schedule.SlotTemplateId = _context.SlotTemplates.FirstOrDefault(s => s.Code == data.TimeSlot).Id;
            return schedule;
        }

        public string IsValid(Schedule schedule)
        {
            ValidationService validationService = new ValidationService(_context);

            return validationService.ValidateSchedule(schedule);
        }

        public void DeleteAllData()
        {
            var allSchedules = _context.Schedules.ToList();
            _context.Schedules.RemoveRange(allSchedules);
            var allSchedulesDetails = _context.ScheduleDetails.ToList();
            _context.ScheduleDetails.RemoveRange(allSchedulesDetails);
            _context.SaveChanges();
        }

        public void DeleteScheduleById(int id)
        {
            var allScheduleDetails = _context.ScheduleDetails.Where(s => s.ScheduleId == id).ToList();
            _context.ScheduleDetails.RemoveRange(allScheduleDetails);

            var schedule = _context.Schedules.FirstOrDefault(s => s.Id == id);
            _context.Schedules.Remove(schedule);
            _context.SaveChanges();
        }
    }
}
