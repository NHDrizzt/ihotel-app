using Microsoft.EntityFrameworkCore;
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

        // 6. Desenvolva o endpoint GET /room/:hotelId
        public IEnumerable<RoomDto> GetRooms(int hotelId)
        {
            var rooms = _context.Rooms.ToList();
            
            var hotel = _context.Hotels
                .Include(h => h.Rooms)
                .Include(h => h.City)
                .FirstOrDefault(h => h.HotelId == hotelId);

            var roomsDto = hotel.Rooms.Select(r => new RoomDto
            {
                roomId = r.RoomId,
                name = r.Name,
                capacity = r.Capacity,
                image = r.Image,
                hotel = new HotelDto
                {
                    hotelId = hotel.HotelId,
                    name = hotel.Name,
                    address = hotel.Address,
                    cityId = hotel.CityId,
                    cityName = hotel.City?.Name,
                    state = hotel.City?.State,
                }
            });
            
            return roomsDto;
        }

        // 7. Desenvolva o endpoint POST /room
        public RoomDto AddRoom(Room room) {
            var newRoom = _context.Rooms.Add(room);
            _context.SaveChanges();

            var rooms = _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.Hotel!.City)
                .FirstOrDefault(r=> r.RoomId == newRoom.Entity.RoomId);
            
            var roomDto = new RoomDto
            {
                roomId = rooms.RoomId,
                name = rooms.Name,
                capacity = rooms.Capacity,
                image = rooms.Image,
                hotel = new HotelDto
                {
                    hotelId = rooms.Hotel!.HotelId,
                    name = rooms.Hotel.Name,
                    address = rooms.Hotel.Address,
                    cityId = rooms.Hotel.CityId,
                    cityName = rooms.Hotel?.City?.Name,
                    state = rooms.Hotel?.City?.State
                }
            };
            return roomDto;
        }

        // 8. Desenvolva o endpoint DELETE /room/:roomId
        public void DeleteRoom(int RoomId) {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == RoomId);
            _context.Rooms.Remove(room!);
            _context.SaveChanges();
        }
    }
}