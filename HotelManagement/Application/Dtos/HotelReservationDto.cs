using HotelManagement.Domain.Enums;
using HotelManagement.Domain.Models;
using static HotelManagement.Domain.Enums.States;

namespace HotelManagement.Application.Dtos
{
    public class HotelReservationDto
    {
        public int IdRoom { get; set; }
        public List<Customer> Customer { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime FinalDate { get; set; }
        public EmergencyContact EmergencyContact { get; set; }
    }

    public class ReservationStatusDto
    {
        public int IdReservation { get; set; }
        public State State { get; set; }
    }
}
