using System;
using System.Collections.Generic;

namespace PRN221_ASSIGNMENT.Models
{
    public partial class Building
    {
        public Building()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Details { get; set; } = null!;

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
