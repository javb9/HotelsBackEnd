using HotelManagement.Domain.Models;
using static HotelManagement.Application.Dtos.HotelManagementDto;

namespace HotelManagement.Domain.Interfaces
{
    public interface IHotelManagement
    {
        Response<int> CreateHotel(Hotel HotelData);
        Response<Hotel> UpdateHotel(Hotel HotelData);
        Response<string> UpdateStatusHotel(HotelDto HotelDto);
        Response<int> CreateRoom(Room RoomData);
        Response<Room> UpdateRoom(Room RoomData);
        Response<string> UpdateStatusRoom(RoomDto RoomDto);
    }
}
