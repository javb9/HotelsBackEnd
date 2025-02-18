using HotelManagement.Application.Dtos;
using HotelManagement.Domain.Interfaces;
using HotelManagement.Domain.Models;

namespace HotelManagement.Application.Services
{
    public class HotelReservationService
    {
        private readonly IHotelReservation _hotelreservation;
        private readonly IEmail _email;

        public HotelReservationService(IHotelReservation hotelreservation, IEmail email) 
        {
            _hotelreservation = hotelreservation;
            _email = email;
        }

        public Response<List<Dictionary<string, string>>> AvailableRooms(DateTime initDate, DateTime finalDate, int numberOfPeople, string City)
        {
            var result = _hotelreservation.AvailableRooms(initDate, finalDate, numberOfPeople, City);
            return result;
        }

        public Response<string> CreateReservation(HotelReservationDto HotelReservationDto)
        {
            var result = _hotelreservation.CreateReservation(HotelReservationDto);
            if (result.Success) {
                _email.SendEmail(HotelReservationDto.Email, "Reserve Confimation", "¡¡Your reservation is confirmed!!, thank you for choosing us.");
            }
            return result;
        }

        public Response<object> Reservations()
        {
            var result = _hotelreservation.Reservations();

            return result;
        }

        public Response<string> UpdateStatusReservation(ReservationStatusDto ReservationStatusDto)
        {
            var result = _hotelreservation.UpdateStatusReservation(ReservationStatusDto);

            return result;
        }

    }
}
