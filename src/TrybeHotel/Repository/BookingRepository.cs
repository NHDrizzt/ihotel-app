using Microsoft.EntityFrameworkCore;
using TrybeHotel.Models;
using TrybeHotel.Dto;

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
            var addNewBooking = new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                RoomId = booking.RoomId,
            };
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var room = GetRoomById(booking.RoomId);
            if (user == null || room == null)
            {
                throw new Exception("Usuário ou quarto não encontrado");
            }

            if (booking.GuestQuant > room.Capacity)
            {
                throw new InvalidOperationException("Guest quantity over room capacity");
            }
            
            addNewBooking.UserId = user.UserId; 
            _context.Bookings.Add(addNewBooking);
            _context.SaveChanges();
            
            
            return BuildBookingResponse(addNewBooking, room);
        }


        public BookingResponse GetBooking(int bookingId, string email)
        {
            var bookingUser = GetUserBooking(bookingId);
            if (bookingUser == null || bookingUser.User!.Email != email)
            {
                throw new UnauthorizedAccessException();
            }
            var booking = _context.Bookings
                .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
                .ThenInclude(h => h.City)
                .FirstOrDefault(b => b.BookingId == bookingId);
           
            if (booking == null)
            {
                throw new Exception("Reserva não encontrada");
            }

            return BuildBookingResponse(booking, booking.Room);
        }

        public Room? GetRoomById(int roomId)
        {
            return _context.Rooms
                .Include(r => r.Hotel)
                .ThenInclude(h => h.City)
                .FirstOrDefault(r => r.RoomId == roomId);
        }

        private Booking? GetUserBooking(int bookingId)
        {
            return _context.Bookings
                .Include(b => b.User)
                .FirstOrDefault(b => b.BookingId == bookingId);
        }
        
        private BookingResponse BuildBookingResponse(Booking booking, Room room)
        {
            if (booking == null || room == null)
            {
                throw new ArgumentNullException("Booking and room must not be null.");
            }

            var bookingResponse = new BookingResponse
            {
                bookingId = booking.BookingId,
                checkIn = booking.CheckIn,
                checkOut = booking.CheckOut,
                guestQuant = booking.GuestQuant,
                room = new RoomDto
                {
                    roomId = room.RoomId,
                    name = room.Name,
                    capacity = room.Capacity,
                    image = room.Image,
                    hotel = new HotelDto
                    {
                        hotelId = room.HotelId,
                        name = room.Hotel.Name,
                        address = room.Hotel.Address,
                        cityId = room.Hotel.CityId,
                        cityName = room.Hotel.City?.Name,
                        state = room.Hotel.City?.State
                    }
                }
            };

            return bookingResponse;
        }

    }

}