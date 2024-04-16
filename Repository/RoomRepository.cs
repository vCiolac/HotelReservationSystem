using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            var rooms = _context.Rooms
                .Where(r => r.HotelId == HotelId)
                .Select(r => new RoomDto
                {
                    RoomId = r.RoomId,
                    Name = r.Name,
                    Capacity = r.Capacity,
                    Image = r.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = r.Hotel != null ? r.Hotel.HotelId : 0,
                        Name = r.Hotel != null ? r.Hotel.Name : null,
                        Address = r.Hotel != null ? r.Hotel.Address : null,
                        CityId = r.Hotel != null ? r.Hotel.CityId : 0,
                        CityName = r.Hotel != null && r.Hotel.City != null ? r.Hotel.City.Name : null,
                        State = r.Hotel != null && r.Hotel.City != null ? r.Hotel.City.State : null
                    }
                })
                .ToList();

            return rooms;
        }

        public RoomDto AddRoom(Room room)
        {
            var hasThisRoom = _context.Rooms.FirstOrDefault(r => r.Name == room.Name);

            if (hasThisRoom != null)
            {
                throw new Exception("Room already exists.");
            }

            _context.Rooms.Add(room);
            _context.SaveChanges();

            var addedRoom = _context.Rooms
                .Where(r => r.RoomId == room.RoomId)
                .Select(r => new RoomDto
                {
                    RoomId = r.RoomId,
                    Name = r.Name,
                    Capacity = r.Capacity,
                    Image = r.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = r.Hotel != null ? r.Hotel.HotelId : 0,
                        Name = r.Hotel != null ? r.Hotel.Name : null,
                        Address = r.Hotel != null ? r.Hotel.Address : null,
                        CityId = r.Hotel != null ? r.Hotel.CityId : 0,
                        CityName = r.Hotel != null && r.Hotel.City != null ? r.Hotel.City.Name : null,
                        State = r.Hotel != null && r.Hotel.City != null ? r.Hotel.City.State : null
                    }
                })
                .FirstOrDefault() ?? throw new Exception("Error trying to get the new added Room.");
            return addedRoom;
        }

        public void DeleteRoom(int RoomId)
        {
            var roomToRemove = _context
            .Rooms
            .FirstOrDefault(r => r.RoomId == RoomId) ?? throw new Exception("Room not found.");
            _ = _context.Rooms.Remove(roomToRemove);
            _context.SaveChanges();
        }
    }
}