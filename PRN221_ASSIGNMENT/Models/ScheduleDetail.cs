using System;
using System.Collections.Generic;

namespace PRN221_ASSIGNMENT.Models
{
    public partial class ScheduleDetail
    {
        public int? ScheduleId { get; set; }
        public DateTime? Date { get; set; }
        public int? SlotId { get; set; }
        public int Id { get; set; }
        public bool? IsChanged { get; set; }

        public virtual Schedule? Schedule { get; set; }
        public virtual Slot? Slot { get; set; }
    }
}
