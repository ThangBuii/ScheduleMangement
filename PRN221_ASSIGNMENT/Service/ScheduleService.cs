using Microsoft.EntityFrameworkCore;
using PRN221_ASSIGNMENT.DTO;
using PRN221_ASSIGNMENT.Models;

namespace PRN221_ASSIGNMENT.Service
{
    public class ScheduleService
    {
        private readonly ScheduleManagementContext _context;

        private ScheduleDetailService detailService;

        public ScheduleService(ScheduleManagementContext context)
        {
            _context = context;
            detailService = new ScheduleDetailService(_context);
        }

        //Check if the data is existed in the database
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
            if (_context.SlotTemplates.FirstOrDefault(s => s.Code == data.TimeSlot) == null) return 5;

            return 0;
        }

        //Tranform Code to Id for Schedule
        public Schedule GetScheduleFromData(CsvData data)
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

        //Check isValid for Schedule
        public string IsValid(Schedule schedule, int ScheduleId)
        {
            ValidationService validationService = new ValidationService(_context);

            return validationService.ValidateSchedule(schedule,ScheduleId);
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

        private int SaveToDB(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            _context.SaveChanges();

            return schedule.Id;
        }

        public void Add(Schedule schedule,List<ScheduleDetail> schedules)
        {
            int newScheduleId = SaveToDB(schedule);
            detailService.SaveToDb(schedules, newScheduleId);
        }

        

        public void Edit(Schedule schedule, List<ScheduleDetail> schedules,int id)
        {
            bool isSlotChanged = false;
            Schedule oldSchedule = _context.Schedules.FirstOrDefault(s => s.Id == id);
            if(schedule.SlotTemplateId != oldSchedule.SlotTemplateId)
            {
                isSlotChanged = true;
            }
            oldSchedule.TeacherId = schedule.TeacherId;
            oldSchedule.SubjectId = schedule.SubjectId;
            oldSchedule.RoomId = schedule.RoomId;
            oldSchedule.ClassId = schedule.ClassId;
            oldSchedule.SlotTemplateId = schedule.SlotTemplateId;

            
            if (isSlotChanged)
            {
                var oldSchedules = _context.ScheduleDetails.Where(s => s.ScheduleId == id).ToList();
                for(int i = 0; i < schedules.Count(); i++)
                {
                    oldSchedules[i].SlotId = schedules[i].SlotId;
                    oldSchedules[i].Date = schedules[i].Date;
                    _context.Entry(oldSchedules[i]).State = EntityState.Modified;
                }

                
            }

            _context.SaveChanges();
        }
    }
}
