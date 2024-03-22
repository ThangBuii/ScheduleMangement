using System;
using System.Collections.Generic;

namespace PRN221_ASSIGNMENT.Models
{
    public partial class Schedule
    {
        public Schedule()
        {
            ScheduleDetails = new HashSet<ScheduleDetail>();
        }

        public int Id { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int RoomId { get; set; }
        public int? SlotTemplateId { get; set; }

        public virtual GroupClass Class { get; set; } = null!;
        public virtual Room Room { get; set; } = null!;
        public virtual SlotTemplate? SlotTemplate { get; set; }
        public virtual Subject Subject { get; set; } = null!;
        public virtual Teacher Teacher { get; set; } = null!;
        public virtual ICollection<ScheduleDetail> ScheduleDetails { get; set; }
    }
}
