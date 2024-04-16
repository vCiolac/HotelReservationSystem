using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            if (booking == null) throw new ArgumentNullException("Booking is null");

            var chooseRoom = GetRoomById(booking.RoomId);
            if (chooseRoom.Capacity < booking.GuestQuant) throw new ArgumentException("Guest quantity over room capacity");

            var bookingEntity = new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                RoomId = booking.RoomId,
            };
            _context.Bookings.Add(bookingEntity);
            _context.SaveChanges();

            var hotel = _context.Hotels
             .Include(h => h.City)
             .FirstOrDefault(x => x.HotelId == chooseRoom.HotelId);

            return new BookingResponse
            {
                BookingId = bookingEntity.BookingId,
                CheckIn = bookingEntity.CheckIn,
                CheckOut = bookingEntity.CheckOut,
                GuestQuant = bookingEntity.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = chooseRoom.RoomId,
                    Name = chooseRoom.Name,
                    Capacity = chooseRoom.Capacity,
                    Image = chooseRoom.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = hotel.HotelId,
                        Name = hotel.Name,
                        Address = hotel.Address,
                        CityName = hotel.City?.Name,
                        CityId = hotel.CityId,
                        State = hotel.City?.State,
                    }
                }
            };

        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email) ?? throw new UnauthorizedAccessException("User not found");

            var choosedRoom = GetRoomById(bookingId);
            var booking = _context.Bookings
              .Where(x => x.BookingId == bookingId && x.UserId == user.UserId)
              .Select(y => new BookingResponse
              {
                  BookingId = y.BookingId,
                  CheckIn = y.CheckIn,
                  CheckOut = y.CheckOut,
                  GuestQuant = y.GuestQuant,
                  Room = new RoomDto
                  {
                      RoomId = y.Room.RoomId,
                      Name = y.Room.Name,
                      Capacity = y.Room.Capacity,
                      Image = y.Room.Image,
                      Hotel = new HotelDto
                      {
                          HotelId = y.Room.HotelId,
                          Name = y.Room.Hotel.Name,
                          Address = y.Room.Hotel.Address,
                          CityName = y.Room.Hotel.City.Name,
                          CityId = y.Room.Hotel.CityId,
                          State = y.Room.Hotel.City.State,
                      }
                  }
              }).FirstOrDefault();

            return booking ?? throw new UnauthorizedAccessException("Booking not found");
        }

        public Room GetRoomById(int RoomId)
        {
            var room = _context.Rooms.FirstOrDefault(x => x.RoomId == RoomId);
            return room ?? throw new ArgumentNullException("Room not found");
        }

    }

}