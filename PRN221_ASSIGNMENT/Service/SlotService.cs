using PRN221_ASSIGNMENT.DTO;
using PRN221_ASSIGNMENT.Models;

namespace PRN221_ASSIGNMENT.Service
{
    public class SlotService
    {
        private string[] validSlots = { "A24", "A35", "A46", "A52", "A63", "P23", "P35", "P46", "P53", "P63" };
        private static DateTime InitialDate = new DateTime(2024, 04, 28);
        private ScheduleManagementContext _context;
        public SlotService(ScheduleManagementContext context)
        {
            _context = context;
        }

        public bool CheckExists(string slotName)
        {
            return Array.IndexOf(validSlots, slotName) != -1;
        }

        public DateTime FindInitialDate(char date)
        {
            int dayOfWeek = int.Parse(date.ToString());
            int daysToAdd = dayOfWeek - 2;

            daysToAdd = (daysToAdd + 7) % 7;

            return InitialDate.AddDays(daysToAdd);
        }

        public DateTime FindNextDate(int weekToSkip, DateTime date)
        {
            int dayToSkip = weekToSkip * 7;

            return date.AddDays(dayToSkip);
        }
    }
}
