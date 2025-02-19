using HotelManagement.Domain.Enums;
using System.Text.Json.Serialization;

namespace HotelManagement.Domain.Models
{
    public class Customer
    {
        [JsonIgnore]
        public int IdCustomer { get; set; }
        [JsonIgnore]
        public int IdReservation { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public Gender Gender { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Email {  get; set; }
        public string PhoneNumber { get; set; }
        public CustomerType CustomerType { get; set; }

        [JsonIgnore]
        public Reservation? Reservations { get; set; }
    }
}
