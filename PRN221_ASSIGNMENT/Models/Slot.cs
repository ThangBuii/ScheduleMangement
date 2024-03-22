using System;
using System.Collections.Generic;

namespace PRN221_ASSIGNMENT.Models
{
    public partial class Slot
    {
        public Slot()
        {
            ScheduleDetails = new HashSet<ScheduleDetail>();
        }

        public int Id { get; set; }
        public string Details { get; set; } = null!;
        public string SlotName { get; set; } = null!;

        public virtual ICollection<ScheduleDetail> ScheduleDetails { get; set; }
    }
}
