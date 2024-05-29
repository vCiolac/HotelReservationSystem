using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;


namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<HotelDto> GetHotels()
        {
            var hotels = _context.Hotels
                .Include(h => h.City)
                .Select(h => new HotelDto
                {
                    HotelId = h.HotelId,
                    Name = h.Name,
                    Address = h.Address,
                    CityId = h.CityId,
                    CityName = h.City != null ? h.City.Name : null,
                    State = h.City != null ? h.City.State : null,
                    Image = h.Image
                });

            return hotels;
        }

        public HotelDto AddHotel(Hotel hotel)
        {
            var hasThisHotel = _context.Hotels.FirstOrDefault(h => h.Name == hotel.Name);

            if (hasThisHotel != null)
            {
                throw new Exception("Hotel already exists.");
            }

            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            var addedHotel = _context.Hotels
                .Include(h => h.City)
                .FirstOrDefault(h => h.HotelId == hotel.HotelId) ?? throw new Exception("Error trying to get the new added Hotel.");
            return new HotelDto
            {
                HotelId = addedHotel.HotelId,
                Name = addedHotel.Name,
                Address = addedHotel.Address,
                CityId = addedHotel.CityId,
                CityName = addedHotel.City?.Name,
                State = addedHotel.City?.State,
                Image = addedHotel.Image
            };
        }
    }
}