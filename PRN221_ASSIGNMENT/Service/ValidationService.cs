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

        public string CheckSlotAndRoom(Schedule schedule, int existedId)
        {
            var roomAndSlot = _context.Schedules
                                        .Include(s => s.SlotTemplate)
                                        .Include(s => s.Room)
                                            .ThenInclude(r => r.Building)
                                        .FirstOrDefault(s => s.SlotTemplateId == schedule.SlotTemplateId && s.RoomId == schedule.RoomId);
            if (roomAndSlot != null && roomAndSlot.Id != existedId)
            {
                return $"Data error: Room {roomAndSlot.Room.Building.Code}-{roomAndSlot.Room.Code} đã được sử dụng vào slot {roomAndSlot.SlotTemplate.Code}." +
                    $" Check Schedule Id = {roomAndSlot.Id}";
            }

            return "";
        }

        public string CheckSlotAndTeacher(Schedule schedule, int existedId)
        {
            var teacherAndSlot = _context.Schedules
                                        .Include(s => s.SlotTemplate)
                                        .Include(s => s.Teacher)
                                        .FirstOrDefault(s => s.SlotTemplateId == schedule.SlotTemplateId && s.TeacherId == schedule.TeacherId);
            if (teacherAndSlot != null && teacherAndSlot.Id != existedId)
            {
                return $"Data error: Teacher {teacherAndSlot.Teacher.Code} đã có lịch dạy vào slot {teacherAndSlot.SlotTemplate.Code}." +
                    $" Check Schedule Id = {teacherAndSlot.Id}";
            }
            return "";
        }

        public string CheckSlotAndClass(Schedule schedule, int existedId)
        {
            var classAndSlot = _context.Schedules
                                       .Include(s => s.SlotTemplate)
                                       .Include(s => s.Class)
                                       .FirstOrDefault(s => s.SlotTemplateId == schedule.SlotTemplateId && s.ClassId == schedule.ClassId);
            if (classAndSlot != null && classAndSlot.Id != existedId)
            {
                return $"Data error: Class {classAndSlot.Class.Code} đã được sử dụng vào slot {classAndSlot.SlotTemplate.Code}" +
                    $" Check Schedule Id = {classAndSlot.Id}";
            }
            return "";
        }

        public string CheckClassAndSubject(Schedule schedule,int existedId)
        {
            var classAndSubject = _context.Schedules
                                        .Include(s => s.Subject)
                                        .Include(s => s.Class)
                                        .FirstOrDefault(s =>  s.ClassId == schedule.ClassId && s.SubjectId == schedule.SubjectId);
            if (classAndSubject != null && classAndSubject.Id != existedId)
            {
               
                return $"Data error: Class {classAndSubject.Class.Code} đã có Subject {classAndSubject.Subject.Code} trong kì này." +
                    $" Check Schedule Id = {classAndSubject.Id}";
            }

            return "";
        }

        public string ValidateSchedule(Schedule schedule, int id)
        {
            string message = "";

            message = CheckClassAndSubject(schedule,id);
            if(message != "")
            {
                return message;
            }

            message = CheckSlotAndRoom(schedule,id);
            if (message != "")
            {
                return message;
            }

            message = CheckSlotAndTeacher(schedule,id);
            if (message != "")
            {
                return message;
            }

            message = CheckSlotAndClass(schedule,id);
            if (message != "")
            {
                return message;
            }

            return message;
        }

        public string ValidateScheduleDetails(List<ScheduleDetail> schedules,Schedule schedule, int id)
        {
            string message = "";

            foreach(var s in schedules)
            {
                message = CheckSlotAndRoomDetail(s, schedule,id);
                if (message != "")
                {
                    return message;
                }

                message = CheckSlotAndTeacherDetail(s, schedule,id);
                if (message != "")
                {
                    return message;
                }

                message = CheckSlotAndClassDetail(s, schedule,id);
                if (message != "")
                {
                    return message;
                }
            }
            return message;
        }

        private string CheckSlotAndClassDetail(ScheduleDetail data, Schedule schedule,int existedId)
        {
            var scheduleDetail = _context.ScheduleDetails.Include(s => s.Schedule).ThenInclude(s => s.Class).Include(s => s.Slot).Where(s => s.IsChanged == true)
                                                .FirstOrDefault(s => s.Date == data.Date && s.Slot.SlotName == data.Slot.SlotName && s.Schedule.Class.Id== schedule.ClassId);
            if (scheduleDetail != null && scheduleDetail.ScheduleId != existedId)
            {
                return $"Data error: Lớp {scheduleDetail.Schedule.Class.Code} đã có lịch học vào slot {scheduleDetail.Slot.SlotName} trong ngày {scheduleDetail.Date.Value.ToString("dd/MM/yyyy")}" +
                    $" Check Schedule Detail Id = {scheduleDetail.Id}";
            }

            return "";
        }
        
        

        private string CheckSlotAndTeacherDetail(ScheduleDetail data, Schedule schedule, int existedId)
        {
            var scheduleDetail = _context.ScheduleDetails.Include(s => s.Schedule).ThenInclude(s => s.Teacher).Include(s => s.Slot).Where(s => s.IsChanged == true)
                                               .FirstOrDefault(s => s.Date == data.Date && s.Slot.SlotName == data.Slot.SlotName && s.Schedule.Teacher.Id == schedule.TeacherId);
            if (scheduleDetail != null && scheduleDetail.ScheduleId != existedId)
            {
                return $"Data error: Teacher {scheduleDetail.Schedule.Teacher.Code} đã có lịch dạy vào slot {scheduleDetail.Slot.SlotName} trong ngày {scheduleDetail.Date.Value.ToString("dd/MM/yyyy")}" +
                    $" Check Schedule Detail Id = {scheduleDetail.Id}";
            }

            return "";
        }

        private string CheckSlotAndRoomDetail(ScheduleDetail data, Schedule schedule, int existedId)
        {
            var scheduleDetail = _context.ScheduleDetails.Include(s => s.Schedule).ThenInclude(s => s.Room).ThenInclude(r => r.Building).Include(s => s.Slot).Where(s => s.IsChanged == true)
                                               .FirstOrDefault(s => s.Date == data.Date && s.Slot.SlotName == data.Slot.SlotName && s.Schedule.Room.Id == schedule.RoomId);
            if (scheduleDetail != null && scheduleDetail.ScheduleId != existedId)
            {
                return $"Data error: Room {scheduleDetail.Schedule.Room.Building.Code}-{scheduleDetail.Schedule.Room.Code} đã được sử dụng vào slot {scheduleDetail.Slot.SlotName} trong ngày {scheduleDetail.Date.Value.ToString("dd/MM/yyyy")}" +
                    $" Check Schedule Detail Id = {scheduleDetail.Id}";
            }

            return "";
        }
    }
}
