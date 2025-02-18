using HotelManagement.Application.Dtos;
using HotelManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;
using static HotelManagement.Application.Dtos.HotelManagementDto;

namespace HotelManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelReservationController : Controller
    {
        private readonly HotelReservationService _hotelreservationservice;
        public HotelReservationController(HotelReservationService hotelreservationservice) 
        {
            _hotelreservationservice = hotelreservationservice;
        }

        /// <summary>
        /// available rooms according to filters
        /// </summary>
        /// <param name="initDate"></param>
        /// <param name="finalDate"></param>
        /// <param name="numberOfPeople"></param>
        /// <param name="City"></param>
        /// <returns></returns>
        [HttpGet("AvailableRooms")]
        public IActionResult AvailableRooms([FromQuery] DateTime initDate, DateTime finalDate, int numberOfPeople, string City)
        {
            var result = _hotelreservationservice.AvailableRooms(initDate, finalDate, numberOfPeople, City);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        /// <summary>
        /// new Reservation for client
        /// </summary>
        /// <param name="HotelReservationDto"></param>
        /// <returns></returns>
        [HttpPost("CreateReservation")]
        public IActionResult CreateReservation([FromBody] HotelReservationDto HotelReservationDto)
        {
            var result = _hotelreservationservice.CreateReservation(HotelReservationDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        /// <summary>
        /// Get Reserves created
        /// </summary>
        /// <returns></returns>
        [HttpGet("Reservations")]
        public IActionResult Reservations()
        {
            var result = _hotelreservationservice.Reservations();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        /// <summary>
        /// update status for reservations, active = 1 and inactive = 0
        /// </summary>
        /// <param name="ReservationStatusDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateStatusReservation")]
        public IActionResult UpdateStatusReservation([FromBody] ReservationStatusDto ReservationStatusDto)
        {
            var result = _hotelreservationservice.UpdateStatusReservation(ReservationStatusDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
