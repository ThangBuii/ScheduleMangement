using System;
using System.Collections.Generic;

namespace PRN221_ASSIGNMENT.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Details { get; set; } = null!;

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
