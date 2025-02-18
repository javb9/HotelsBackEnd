using System.Text.Json.Serialization;
using static HotelManagement.Domain.Enums.States;

namespace HotelManagement.Domain.Models
{
    public class Room
    {
        public int IdRoom { get; set; }
        public int IdHotel { get; set; }
        public string Number { get; set; }
        public decimal BaseCost { get; set; }
        public decimal Tax { get; set; }
        public string RoomType { get; set; }
        public string Ubication { get; set; }
        public int Capacity { get; set; }
        public State State { get; set; }

        [JsonIgnore]
        public Hotel? Hotel { get; set; }

        [JsonIgnore]
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
