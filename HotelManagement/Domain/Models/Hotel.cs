using System.Text.Json.Serialization;
using static HotelManagement.Domain.Enums.States;

namespace HotelManagement.Domain.Models
{
    public class Hotel
    {
        public int IdHotel { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal comision { get; set; }
        public string Ubication { get; set; }
        public State State { get; set; }

        [JsonIgnore]
        public ICollection<Room>? Rooms { get; set; }
    }
}
