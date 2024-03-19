using Microsoft.EntityFrameworkCore;
using PRN221_ASSIGNMENT.DTO;
using PRN221_ASSIGNMENT.Models;

namespace PRN221_ASSIGNMENT.Mapper
{
    public class RoomMapper
    {
        private readonly ScheduleManagementContext _context;
        public RoomMapper(ScheduleManagementContext context)
        {
            _context = context;
        }

        public List<RoomDTO> ToRoomDTO()
        {
            List<Room> rooms = _context.Rooms.Include(r => r.Building).ToList();
            List<RoomDTO> results = new List<RoomDTO>();
            foreach (Room room in rooms)
            {
                results.Add(new RoomDTO
                {
                    Room = $"{room.Building.Code}-{room.Code}"
                });
            }

            return results;
        }
    }
}
