using HotelManagement.Domain.Interfaces;
using HotelManagement.Domain.Models;
using static HotelManagement.Application.Dtos.HotelManagementDto;

namespace HotelManagement.Application.Services
{
    public class HotelManagementService 
    {
        private readonly IHotelManagement _hotelmanagement;
        
        public HotelManagementService(IHotelManagement hotelmanagement)
        {
            _hotelmanagement = hotelmanagement;
        }

        public Response<int> CreateHotel(Hotel HotelData)
        {
            var result = _hotelmanagement.CreateHotel(HotelData);
            return result;
        }

        public Response<Hotel> UpdateHotel(Hotel HotelData)
        {
            var result = _hotelmanagement.UpdateHotel(HotelData);
            return result;
        }

        public Response<string> UpdateStatusHotel(HotelDto HotelDto)
        {
            var result = _hotelmanagement.UpdateStatusHotel(HotelDto);
            return result;
        }

        public Response<int> CreateRoom(Room RoomData)
        {
            var result = _hotelmanagement.CreateRoom(RoomData);
            return result;
        }

        public Response<Room> UpdateRoom(Room RoomData)
        {
            var result = _hotelmanagement.UpdateRoom(RoomData);
            return result;
        }

        public Response<string> UpdateStatusRoom(RoomDto RoomDto)
        {
            var result = _hotelmanagement.UpdateStatusRoom(RoomDto);
            return result;
        }
    }
}
