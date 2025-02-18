using HotelManagement.Application.Services;
using HotelManagement.Domain.Interfaces;
using HotelManagement.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static HotelManagement.Application.Dtos.HotelManagementDto;

namespace HotelManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelManagementController : Controller
    {
        private readonly HotelManagementService _hotelmanagementservice;
        public HotelManagementController(HotelManagementService hotelmanagementservice)
        {
            _hotelmanagementservice = hotelmanagementservice;
        }

        /// <summary>
        /// Create new hotel
        /// </summary>
        /// <param name="HotelData"></param>
        /// <returns></returns>
        [HttpPost("CreateHotel")]
        public IActionResult CreateHotel([FromBody] Hotel HotelData)
        {
            var result = _hotelmanagementservice.CreateHotel(HotelData);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        /// <summary>
        /// Update Hotel
        /// </summary>
        /// <param name="HotelData"></param>
        /// <returns></returns>
        [HttpPut("UpdateHotel")]
        public IActionResult UpdateHotel([FromBody] Hotel HotelData)
        {
            var result = _hotelmanagementservice.UpdateHotel(HotelData);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        /// <summary>
        /// update status for Hotels, active = 1 and inactive = 0
        /// </summary>
        /// <param name="HotelDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateStatusHotel")]
        public IActionResult UpdateStatusHotel([FromBody] HotelDto HotelDto)
        {
            var result = _hotelmanagementservice.UpdateStatusHotel(HotelDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        /// <summary>
        /// Create new Room
        /// </summary>
        /// <param name="RoomData"></param>
        /// <returns></returns>
        [HttpPost("CreateRoom")]
        public IActionResult CreateRoom([FromBody] Room RoomData)
        {
            var result = _hotelmanagementservice.CreateRoom(RoomData);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        /// <summary>
        /// Update Room
        /// </summary>
        /// <param name="RoomData"></param>
        /// <returns></returns>
        [HttpPut("UpdateRoom")]
        public IActionResult UpdateRoom([FromBody] Room RoomData)
        {
            var result = _hotelmanagementservice.UpdateRoom(RoomData);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        /// <summary>
        /// update status for Rooms, active = 1 and inactive = 0
        /// </summary>
        /// <param name="RoomDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateStatusRoom")]
        public IActionResult UpdateStatusRoom([FromBody] RoomDto RoomDto)
        {
            var result = _hotelmanagementservice.UpdateStatusRoom(RoomDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

    }
}
