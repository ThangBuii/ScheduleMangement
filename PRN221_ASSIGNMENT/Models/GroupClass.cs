using System;
using System.Collections.Generic;

namespace PRN221_ASSIGNMENT.Models
{
    public partial class GroupClass
    {
        public GroupClass()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
