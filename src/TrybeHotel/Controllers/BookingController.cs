using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;
using TrybeHotel.ExceptionHandler;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]
  
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(Policy = "client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert){
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)!.Value;
                var booking = _repository.Add(bookingInsert, email);
                return StatusCode(201, booking);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
            catch(Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }


        [HttpGet("{Bookingid}")]
        [Authorize(Policy = "client")]
        public IActionResult GetBooking(int bookingId){
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)!.Value;
                var booking = _repository.GetBooking(bookingId, email);
                return Ok(booking);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
            catch(Exception ex)
            {
                return Unauthorized(new ErrorResponse(ex.Message));
            }
        }
    }
}