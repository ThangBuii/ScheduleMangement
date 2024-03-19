
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
        private ScheduleDTO GetInitialData(CsvData data)
        {
            string[] roomParts = data.Room.Split('-');
            string buildingCode = roomParts[0];
            string roomCode = roomParts[1];

            Schedule Schedule1 = new Schedule();
            Schedule Schedule2 = new Schedule();

            Schedule1.TeacherId = _context.Teachers.FirstOrDefault(t => t.Code == data.Teacher).Id;
            Schedule2.TeacherId = Schedule1.TeacherId;

            Schedule1.RoomId = _context.Rooms.Include(r => r.Building)
                                             .FirstOrDefault(r => r.Building.Code == buildingCode && r.Code == roomCode).Id;
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

            return new ScheduleDTO
            {
                Schedule1 = Schedule1,
                Schedule2 = Schedule2,
            };
        }

        public List<Schedule> GetListSchedule(ScheduleDTO scheduleDTO)
        {
            List<Schedule> schedules = new List<Schedule>();
            Schedule schedule1 = scheduleDTO.Schedule1;

            Schedule schedule2 = scheduleDTO.Schedule2;

            for (int i = 1; i < 10; i++)
            {
                Schedule newSchedule1 = copySchedule(schedule1);
                newSchedule1.Date = slotService.FindNextDate(i, schedule1.Date);
                schedules.Add(newSchedule1);
                Schedule newSchedule2 = copySchedule(schedule2);
                newSchedule2.Date = slotService.FindNextDate(i, schedule2.Date);
                schedules.Add(newSchedule2);
            }

            return schedules;
        }

        private Schedule copySchedule(Schedule initial)
        {
            return new Schedule
            {
                ClassId = initial.ClassId,
                SubjectId = initial.SubjectId,
                RoomId = initial.RoomId,
                TeacherId = initial.TeacherId,
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
            if (!slotService.CheckExists(data.TimeSlot)) return 5;

            return 0;
        }

        public void SaveToDb(List<Schedule> schedules)
        {
            foreach (Schedule s in schedules)
            {
                _context.Add(s);
            }
            _context.SaveChanges();
        }

        public string AddDataToDatabase(CsvData data)
        {
            int messageId = IsExist(data);
            if (messageId == 0)
            {

                ScheduleDTO scheduleDTO = GetInitialData(data);
                string message = IsValid(scheduleDTO.Schedule1);
                if (message != "")
                {
                    return message;
                }
                try
                {
                    _context.Schedules.Add(scheduleDTO.Schedule1);
                    _context.Schedules.Add(scheduleDTO.Schedule2);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return FindConstraintError(scheduleDTO.Schedule1);
                }
                List<Schedule> schedules = GetListSchedule(scheduleDTO);

                SaveToDb(schedules);
            }
            else
            {
                if (messageId == 1)
                {
                    return "Teacher code do not exists";
                }
                else if (messageId == 2)
                {
                    return "Subject code do not exists";
                }
                else if (messageId == 3)
                {
                    return "Class code do not exists";
                }
                else if (messageId == 4)
                {
                    return "Room code do not exists";
                }
                else if (messageId == 5)
                {
                    return "Wrong slot template!";
                }
            }


            return "Saved successfully!";
        }

        public string IsValid(Schedule schedule)
        {
            ValidationService validationService = new ValidationService(_context);

            return validationService.CheckClassAndSubjectAll(schedule);
        }

        public string FindConstraintError(Schedule schedule)
        {
            ValidationService validationService = new ValidationService(_context);
            string message;
            message = validationService.CheckSlotAndRoom(schedule);
            if (message != "") return message;

            message = validationService.CheckSlotAndTeacher(schedule);
            if (message != "") return message;

            message = validationService.CheckSlotAndClass(schedule);
            if (message != "") return message;

            message = validationService.CheckClassAndSubject(schedule);
            if (message != "") return message;

            return "";
        }

        public void DeleteAllData()
        {
            var allSchedules = _context.Schedules.ToList();
            _context.Schedules.RemoveRange(allSchedules);
            _context.SaveChanges();
        }
    }
}
