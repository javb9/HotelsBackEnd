using HotelManagement.Domain.Enums;

namespace HotelManagement.Application.Dtos
{
    public class HotelReservationDto
    {
        public int IdRoom { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public Gender Gender { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime FinalDate { get; set; }
        public string CompleteNameEmergencyContact { get; set; }
        public string PhoneNumberEmergencyContact { get; set; }
    }
}
