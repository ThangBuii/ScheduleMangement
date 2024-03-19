using Microsoft.EntityFrameworkCore;
using PRN221_ASSIGNMENT.Models;

namespace PRN221_ASSIGNMENT.Service
{
    public class ValidationService
    {
        private ScheduleManagementContext _context;

        public ValidationService(ScheduleManagementContext context)
        {
            _context = context;
        }

        public string CheckSlotAndRoom(Schedule schedule)
        {
            var roomAndSlot = _context.Schedules
                                        .Include(s => s.Slot)
                                        .Include(s => s.Room)
                                            .ThenInclude(r => r.Building)
                                        .FirstOrDefault(s => s.Date == schedule.Date && s.SlotId == schedule.SlotId && s.RoomId == schedule.RoomId);
            if (roomAndSlot != null)
            {
                return $"Data error: There is already a schedule at Slot {roomAndSlot.Slot.SlotName} in Room {roomAndSlot.Room.Building.Code}-{roomAndSlot.Room.Code} on the same TimeSlot";
            }

            return "";
        }

        public string CheckSlotAndTeacher(Schedule schedule)
        {
            var teacherAndSlot = _context.Schedules
                                        .Include(s => s.Slot)
                                        .Include(s => s.Teacher)
                                        .FirstOrDefault(s => s.Date == schedule.Date && s.SlotId == schedule.SlotId && s.TeacherId == schedule.TeacherId);
            if (teacherAndSlot != null)
            {
                return $"Data error: There is already a schedule at Slot {teacherAndSlot.Slot.SlotName} taught by Teacher {teacherAndSlot.Teacher.Code} on the same TimeSlot";
            }
            return "";
        }

        public string CheckSlotAndClass(Schedule schedule)
        {
            var classAndSlot = _context.Schedules
                                       .Include(s => s.Slot)
                                       .Include(s => s.Class)
                                       .FirstOrDefault(s => s.Date == schedule.Date && s.SlotId == schedule.SlotId && s.ClassId == schedule.ClassId);
            if (classAndSlot != null)
            {
                return $"Data error: There is already a schedule at Slot {classAndSlot.Slot.SlotName} of Class {classAndSlot.Class.Code} on the same TimeSlot";
            }
            return "";
        }

        public string CheckClassAndSubject(Schedule schedule)
        {
            var classAndSubject = _context.Schedules
                                        .Include(s => s.Subject)
                                        .Include(s => s.Class)
                                        .FirstOrDefault(s => s.Date == schedule.Date && s.ClassId == schedule.ClassId && s.SubjectId == schedule.SubjectId);
            if (classAndSubject != null)
            {
                return $"Data error: Class {classAndSubject.Class.Code} already have 1 slot for subject {classAndSubject.Subject.Code} on this TimeSlot";
            }

            return "";
        }

        public string CheckClassAndSubjectAll(Schedule schedule)
        {
            List<Schedule> schedules = new List<Schedule>();
            schedules = _context.Schedules.Include(s => s.Class).Include(s => s.Subject).Where(s => s.ClassId == schedule.ClassId && s.SubjectId == schedule.SubjectId).ToList();
            if(schedules.Count >= 2)
            {
                return $"Data error: Class {schedules[0].Class.Code} already have subject {schedules[0].Subject.Code} in this semester.";
            }

            return "";
        }
    }
}
