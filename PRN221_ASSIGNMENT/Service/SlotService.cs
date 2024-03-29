﻿using PRN221_ASSIGNMENT.DTO;
using PRN221_ASSIGNMENT.Models;
using System.Diagnostics.Eventing.Reader;

namespace PRN221_ASSIGNMENT.Service
{
    public class SlotService
    {
        private string[] validSlots = { "A24", "A35", "A46", "A52", "A63", "P23", "P35", "P46", "P52", "P63" };
        private static DateTime InitialDate = new DateTime(2024, 04, 29);
        private static DateTime EndDate = new DateTime(2024, 07, 14);
        private ScheduleManagementContext _context;
        public SlotService(ScheduleManagementContext context)
        {
            _context = context;
        }

        public bool CheckExists(string slotName)
        {
            foreach (var slot in validSlots)
            {
                if (slotName.Equals(slot)) return true;
            }
            return false;
        }

        public DateTime FindInitialDate(char date)
        {
            int dayOfWeek = int.Parse(date.ToString());
            int daysToAdd = dayOfWeek - 2;

            daysToAdd = (daysToAdd + 7) % 7;

            return InitialDate.AddDays(daysToAdd);
        }

        public List<DateTime> FindDateTimeList()
        {
            // Create a list to store DateTimes
            List<DateTime> DateTimes = new List<DateTime>();

            // Add dates to the list
            DateTime currentDate = InitialDate;
            while (currentDate <= EndDate)
            {
                DateTimes.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }

            return DateTimes;
        }

        public DateTime FindNextDate(int weekToSkip, DateTime date)
        {
            int dayToSkip = weekToSkip * 7;

            return date.AddDays(dayToSkip);
        }

        public List<WeekDTO> FindWeekList()
        {
            List<WeekDTO > weeks = new List<WeekDTO>();
            DateTime currentWeekStart = InitialDate;
            int i = 1;
            while (currentWeekStart <= EndDate)
            {
                DateTime currentWeekEnd = currentWeekStart.AddDays(6); // End of the week is 6 days later

                if (currentWeekEnd > EndDate)
                {
                    currentWeekEnd = EndDate;
                }

                weeks.Add(new WeekDTO { StartDate = currentWeekStart, EndDate = currentWeekEnd ,WeekNo = i });

                // Move to the next Monday
                currentWeekStart = currentWeekEnd.AddDays(1);
                i++;
            }

            return weeks;
        }

        public string ConvertDateToSlot(DateTime date, int slotId)
        {
            string prefix="";
            string suffix="";
            string result;
            DayOfWeek dayOfWeek = date.DayOfWeek;
            if (slotId == 1 || slotId == 2)
            {
                prefix = "A";   
            }
            else
            {
                prefix = "P";
            }

            if (slotId == 1 || slotId == 3)
            {
                if (dayOfWeek == DayOfWeek.Monday)
                {
                    suffix = "24";
                }
                else if (dayOfWeek == DayOfWeek.Tuesday)
                {
                    suffix = "35";
                }
                else if (dayOfWeek == DayOfWeek.Wednesday)
                {
                    suffix = "46";
                }else if (dayOfWeek == DayOfWeek.Thursday)
                {
                    suffix = "52";
                }else if(dayOfWeek == DayOfWeek.Friday)
                {
                    suffix = "63";
                }
            }
            else
            {
                if (dayOfWeek == DayOfWeek.Monday)
                {
                    suffix = "52";
                }
                else if (dayOfWeek == DayOfWeek.Tuesday)
                {
                    suffix = "63";
                }
                else if (dayOfWeek == DayOfWeek.Wednesday)
                {
                    suffix = "24";
                }
                else if (dayOfWeek == DayOfWeek.Thursday)
                {
                    suffix = "35";
                }
                else if (dayOfWeek == DayOfWeek.Friday)
                {
                    suffix = "46";
                }
            }
            result = prefix + suffix;
            return result;
        }

        public List<DateTime> GetDaysBetween(WeekDTO currentWeek)
        {
            List<DateTime> days = new List<DateTime>();
            DateTime startDate = currentWeek.StartDate;
            DateTime endDate = currentWeek.EndDate;
            // Add the start date to the list
            days.Add(startDate);

            // Loop from the day after the start date until the end date
            for (DateTime date = startDate.AddDays(1); date <= endDate; date = date.AddDays(1))
            {
                days.Add(date);
            }

            return days;
        }

        public DateTime FindDateBasedOnWeek(WeekDTO? currentWeek, int dayNo)
        {
            return currentWeek.StartDate.AddDays(dayNo);
        }
    }
}
