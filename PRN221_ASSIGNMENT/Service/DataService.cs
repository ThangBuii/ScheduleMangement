
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
        private ScheduleDetailService scheduleDetailService;
        private ScheduleService scheduleService;
        public DataService(ScheduleManagementContext context)
        {
            _context = context;
            slotService = new SlotService(_context);
            scheduleDetailService = new ScheduleDetailService(_context);
            scheduleService = new ScheduleService(_context);
        }

        public string CheckDataExist(CsvData data)
        {
            string message = "";
            int messageId = scheduleService.IsExist(data);
            if (messageId == 0)
            {
                message = "";
            }
            else if (messageId == 1)
            {
                message = "Teacher code do not exists!";
            }
            else if (messageId == 2)
            {
                message = "Subject code do not exists!";
            }
            else if (messageId == 3)
            {
                message = "Class code do not exists!";
            }
            else if (messageId == 4)
            {
                message = "Room code do not exists!";
            }
            else if (messageId == 5)
            {
                message = "Slot do not exists!";
            }
            return message;
        }

        public string AddDataToDatabase(CsvData data)
        {
            string message = "";
            message = CheckDataExist(data);
            if (message != "")
            {
                return message;
            }
            Schedule schedule = scheduleService.GetScheduleFromData(data);
            message = scheduleService.IsValid(schedule,0);
            if (message != "")
            {
                return message;
            }   
            List<ScheduleDetail> schedules = scheduleDetailService.GetListSchedule(data);
            message = scheduleDetailService.IsValidScheduleDetails(schedules, schedule,0);
            if (message != "")
            {
                return message;
            }

            scheduleService.Add(schedule, schedules);

            message = "Saved successfully!";
            return message;
        }

        public string EditData(CsvData data, int id)
        {
            string message = "";
            message = CheckDataExist(data);
            if (message != "")
            {
                return message;
            }
            Schedule schedule = scheduleService.GetScheduleFromData(data);
            message = scheduleService.IsValid(schedule,id);
            if (message != "")
            {
                return message;
            }
            List<ScheduleDetail> schedules = scheduleDetailService.GetListSchedule(data);
            message = scheduleDetailService.IsValidScheduleDetails(schedules, schedule, id);
            if (message != "")
            {
                return message;
            }
            scheduleService.Edit(schedule, schedules,id);

            message = "Saved successfully!";
            return message;
        }

        public string EditScheduleDetail(ScheduleDetail scheduleDetail)
        {
            string message = "";

            message = scheduleDetailService.IsValidScheduleDetail( scheduleDetail);
            if (message != "")
            {
                return message;
            }
            scheduleDetailService.Edit(scheduleDetail);

            message = "Saved successfully!";
            return message;
        }
    }
}
