using PRN221_ASSIGNMENT.DTO;
using PRN221_ASSIGNMENT.Models;

namespace PRN221_ASSIGNMENT.Service
{
    public class ScheduleDetailService
    {
        private readonly ScheduleManagementContext _context;

        private SlotService slotService;
        public ScheduleDetailService(ScheduleManagementContext context)
        {
            _context = context;
            slotService = new SlotService(_context);
        }

        //Find data for first two Schedule Details
        public ScheduleDTO GetInitialData(CsvData data)
        {

            ScheduleDetail Schedule1 = new ScheduleDetail();
            ScheduleDetail Schedule2 = new ScheduleDetail();

            Schedule1.IsChanged = false;
            Schedule2.IsChanged = false;

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

        //Get 18 ScheduleDetails left
        public List<ScheduleDetail> GetListSchedule(CsvData data)
        {
            ScheduleDTO scheduleDTO = GetInitialData(data);
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

        //Copy scheduleDetails
        private ScheduleDetail copySchedule(ScheduleDetail initial)
        {
            return new ScheduleDetail
            {
                ScheduleId = initial.ScheduleId,
                SlotId = initial.SlotId,
                Date = initial.Date,
                IsChanged = initial.IsChanged,
            };
        }

        //Save scheduleDetails to the database
        public void SaveToDb(List<ScheduleDetail> schedules, int id)
        {
            schedules = AddScheduleIdForScheduleDetails(id, schedules);
            foreach (ScheduleDetail s in schedules)
            {
                _context.ScheduleDetails.Add(s);
            }
            _context.SaveChanges();
        }

        //Add id for Schedule Details
        private List<ScheduleDetail> AddScheduleIdForScheduleDetails(int id, List<ScheduleDetail> list)
        {
            foreach (var s in list)
            {
                s.ScheduleId = id;
            }

            return list;
        }

        public string IsValidScheduleDetails(List<ScheduleDetail> schedules,Schedule schedule,int id)
        {
            ValidationService validationService = new ValidationService(_context);
            if (!IsChangedScheduleDetailExist()) return ""; 
            return validationService.ValidateScheduleDetails(schedules,schedule, id);
        }

        private bool IsChangedScheduleDetailExist()
        {
            if (_context.ScheduleDetails.FirstOrDefault(s => s.IsChanged == true) != null) return true;
            return false;
        }
    }
}
