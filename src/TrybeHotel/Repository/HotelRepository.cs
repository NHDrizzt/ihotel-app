using Microsoft.EntityFrameworkCore;
using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Desenvolva o endpoint GET /hotel
        public IEnumerable<HotelDto> GetHotels()
        {
            var hotels = _context.Hotels
                .Include(hotel => hotel.City) 
                .ToList();
            var hotelDto =  hotels.Select(hotel => new HotelDto
            {
                hotelId = hotel.HotelId,
                name = hotel.Name,
                address = hotel.Address,
                cityId = hotel.CityId,
                cityName = hotel.City?.Name,
                state = hotel.City?.State
            });
            return hotelDto;
        }
        
        // 5. Desenvolva o endpoint POST /hotel
        public HotelDto AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();
            
            var hotelEntity = _context.Hotels
                .Include(h => h.City)
                .FirstOrDefault(h => h.HotelId == hotel.HotelId);
            
            var hotelDto = new HotelDto
            {
                hotelId = hotel.HotelId,
                name = hotel.Name,
                address = hotel.Address,
                cityId = hotel.CityId,
                cityName = hotelEntity.City.Name,
                state = hotelEntity.City.State
            };
            return hotelDto;
        }
    }
}