using HotelManagement.Domain.Enums;
using System.Text.Json.Serialization;
using static HotelManagement.Domain.Enums.States;

namespace HotelManagement.Domain.Models
{
    public class Reservation
    {
        public int IdReservation { get; set; }
        public int IdRoom { get; set; }
        public int IdEmergencyContact {  get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime FinalDate { get; set; }
        public State state { get; set; }

        [JsonIgnore]
        public Room? Room { get; set; }

        [JsonIgnore]
        public ICollection<Customer>? Customer { get; set; }

        [JsonIgnore]
        public EmergencyContact? EmergencyContact { get; set; }

    }
}
