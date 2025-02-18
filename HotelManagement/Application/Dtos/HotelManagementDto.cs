using HotelManagement.Domain.Enums;
using static HotelManagement.Domain.Enums.States;

namespace HotelManagement.Application.Dtos
{
    public class HotelManagementDto
    {
        public class HotelDto
        {
            public int IdHotel { get; set; }
            public State State { get; set; }
        }

        public class RoomDto
        {
            public int IdRoom { get; set; }
            public State State { get; set; }
        }


    }
}
