using System.Text.Json.Serialization;

namespace HotelManagement.Domain.Models
{
    public class EmergencyContact
    {
        [JsonIgnore]
        public int IdEmergencyContact { get; set; }
        public string CompleteName { get; set; }
        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public Reservation? Reservation { get; set; }
    }
}
