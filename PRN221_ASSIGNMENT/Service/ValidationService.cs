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

        
        //public string CheckClassAndSubjectScheduleDetail(Schedule schedule)
        //{
        //    var classAndSubject = _context.Schedules
        //                                .Include(s => s.Subject)
        //                                .Include(s => s.Class)
        //                                .FirstOrDefault(s => s.Date == schedule.Date && s.ClassId == schedule.ClassId && s.SubjectId == schedule.SubjectId);
        //    if (classAndSubject != null)
        //    {
        //        return $"Data error: Class {classAndSubject.Class.Code} already have 1 slot for subject {classAndSubject.Subject.Code} on this TimeSlot";
        //    }

        //    return "";
        //}

        //public string CheckClassAndSubjectAllScheduleDetail(Schedule schedule)
        //{
        //    List<Schedule> schedules = new List<Schedule>();
        //    schedules = _context.Schedules.Include(s => s.Class).Include(s => s.Subject).Where(s => s.ClassId == schedule.ClassId && s.SubjectId == schedule.SubjectId).ToList();
        //    if(schedules.Count >= 2)
        //    {
        //        return $"Data error: Class {schedules[0].Class.Code} already have subject {schedules[0].Subject.Code} in this semester.";
        //    }

        //    return "";
        //}

        public string CheckSlotAndRoom(Schedule schedule)
        {
            var roomAndSlot = _context.Schedules
                                        .Include(s => s.SlotTemplate)
                                        .Include(s => s.Room)
                                            .ThenInclude(r => r.Building)
                                        .FirstOrDefault(s => s.SlotTemplateId == schedule.SlotTemplateId && s.RoomId == schedule.RoomId);
            if (roomAndSlot != null)
            {
                return $"Data error: There is already a schedule at Slot {roomAndSlot.SlotTemplate.Code} in Room {roomAndSlot.Room.Building.Code}-{roomAndSlot.Room.Code}." +
                    $" Conflict with Schedule id = {roomAndSlot.Id}";
            }

            return "";
        }

        public string CheckSlotAndTeacher(Schedule schedule)
        {
            var teacherAndSlot = _context.Schedules
                                        .Include(s => s.SlotTemplate)
                                        .Include(s => s.Teacher)
                                        .FirstOrDefault(s => s.SlotTemplateId == schedule.SlotTemplateId && s.TeacherId == schedule.TeacherId);
            if (teacherAndSlot != null)
            {
                return $"Data error: There is already a schedule at Slot {teacherAndSlot.SlotTemplate.Code} taught by Teacher {teacherAndSlot.Teacher.Code}." +
                    $" Conflict with Schedule id = {teacherAndSlot.Id}";
            }
            return "";
        }

        public string CheckSlotAndClass(Schedule schedule)
        {
            var classAndSlot = _context.Schedules
                                       .Include(s => s.SlotTemplate)
                                       .Include(s => s.Class)
                                       .FirstOrDefault(s => s.SlotTemplateId == schedule.SlotTemplateId && s.ClassId == schedule.ClassId);
            if (classAndSlot != null)
            {
                return $"Data error: There is already a schedule at Slot {classAndSlot.SlotTemplate.Code} of Class {classAndSlot.Class.Code}." +
                    $" Conflict with Schedule id = {classAndSlot.Id}";
            }
            return "";
        }

        public string CheckClassAndSubject(Schedule schedule)
        {
            var classAndSubject = _context.Schedules
                                        .Include(s => s.Subject)
                                        .Include(s => s.Class)
                                        .FirstOrDefault(s =>  s.ClassId == schedule.ClassId && s.SubjectId == schedule.SubjectId);
            if (classAndSubject != null)
            {
                return $"Data error: Class {classAndSubject.Class.Code} already have 1 slot for subject {classAndSubject.Subject.Code}." +
                    $" Conflict with Schedule id = {classAndSubject.Id}";
            }

            return "";
        }

        public string ValidateSchedule(Schedule schedule)
        {
            string message = "";

            message = CheckClassAndSubject(schedule);
            if(message != "")
            {
                return message;
            }

            message = CheckSlotAndRoom(schedule);
            if (message != "")
            {
                return message;
            }

            message = CheckSlotAndTeacher(schedule);
            if (message != "")
            {
                return message;
            }

            message = CheckSlotAndClass(schedule);
            if (message != "")
            {
                return message;
            }

            return message;
        }
    }
}
