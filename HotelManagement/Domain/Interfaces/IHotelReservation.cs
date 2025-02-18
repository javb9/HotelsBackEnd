using HotelManagement.Application.Dtos;
using HotelManagement.Domain.Models;

namespace HotelManagement.Domain.Interfaces
{
    public interface IHotelReservation
    {
        Response<List<Dictionary<string, string>>> AvailableRooms(DateTime initDate, DateTime finalDate, int numberOfPeople, string City);
        Response<string> CreateReservation(HotelReservationDto HotelReservationDto);
        Response<object> Reservations();

    }
}
