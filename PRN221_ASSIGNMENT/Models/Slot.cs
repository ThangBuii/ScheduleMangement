using System;
using System.Collections.Generic;

namespace PRN221_ASSIGNMENT.Models
{
    public partial class Slot
    {
        public Slot()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string Details { get; set; } = null!;
        public string SlotName { get; set; } = null!;

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
