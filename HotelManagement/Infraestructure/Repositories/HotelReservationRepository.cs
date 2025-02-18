using HotelManagement.Application.Data;
using HotelManagement.Application.Dtos;
using HotelManagement.Application.Services;
using HotelManagement.Domain.Interfaces;
using HotelManagement.Domain.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using static HotelManagement.Application.Dtos.HotelManagementDto;
using static HotelManagement.Domain.Enums.States;

namespace HotelManagement.Infraestructure.Repositories
{
    public class HotelReservationRepository : IHotelReservation
    {
        private readonly ApplicationDbContext _context;
        public HotelReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Response<List<Dictionary<string, string>>> AvailableRooms(DateTime initDate, DateTime finalDate, int numberOfPeople, string City)
        {
            try
            {
                List<Dictionary<string, string>> RoomsAvailable = new List<Dictionary<string, string>>();

                if (finalDate < initDate)
                {
                    return new Response<List<Dictionary<string, string>>>(null, false, "The final date cannot be less than the initial date");
                };

                var rooms = ValidateRoomAvailable(initDate, finalDate, numberOfPeople, City);

                if (rooms.Count == 0)
                {
                    return new Response<List<Dictionary<string, string>>>(RoomsAvailable, true, "No rooms available");
                }

                // get days for cost
                TimeSpan differenceDays = finalDate - initDate;

                foreach (var room in rooms)
                {

                    var hotel = _context.Hotels.Where(x => x.IdHotel == room.IdHotel).FirstOrDefault();

                    Dictionary<string, string> Room = new Dictionary<string, string>();

                    Room.Add("IdRoom", room.IdRoom.ToString());
                    Room.Add("IdHotel", hotel.IdHotel.ToString());
                    Room.Add("Hotel", hotel.Name);
                    Room.Add("Room", room.Number);
                    Room.Add("RoomType", room.RoomType);
                    Room.Add("BaseCost", room.BaseCost.ToString());
                    Room.Add("Tax", room.Tax.ToString());
                    Room.Add("Total Cost", ((room.BaseCost * differenceDays.Days) + ((room.BaseCost * differenceDays.Days) * (room.Tax / 100))).ToString());

                    RoomsAvailable.Add(Room);
                }

                return new Response<List<Dictionary<string, string>>>(RoomsAvailable, true, "Rooms available");
            }
            catch (Exception ex)
            {
                return new Response<List<Dictionary<string, string>>>(null, false, "error, please contact IT");
            }
        }

        public Response<string> CreateReservation(HotelReservationDto HotelReservationDto)
        {
            try
            {

                if (HotelReservationDto.FinalDate < HotelReservationDto.InitDate)
                {
                    return new Response<string>("ERROR", false, "The final date cannot be less than the initial date");
                };

                if (HotelReservationDto.IdRoom == null || HotelReservationDto.IdRoom == 0)
                {
                    return new Response<string>("ERROR", false, "Room id must be sent");
                };

                if (_context.Rooms.Count(x => x.IdRoom == HotelReservationDto.IdRoom) == 0)
                {
                    return new Response<string>("ERROR", false, "The Room must be registered");
                }

                // get the room to bring the location of the hotel
                var room = _context.Rooms.Where(x => x.IdRoom == HotelReservationDto.IdRoom).First();

                var rooms = ValidateRoomAvailable(HotelReservationDto.InitDate, HotelReservationDto.FinalDate, HotelReservationDto.NumberOfPeople, _context.Hotels.Where(x=> x.IdHotel == room.IdHotel).First().Ubication);

                if (rooms.Count(x=>x.IdRoom == HotelReservationDto.IdRoom) == 0)
                {
                    return new Response<string>("ERROR", false, "The room is already reserved or the number of persons exceeds the capacity");
                }

                Client client = new Client();

                //validate if exist the client
                client = _context.Client.Where(x => x.DocumentNumber == HotelReservationDto.DocumentNumber).FirstOrDefault();

                if (client == null)
                {
                    client = new Client();
                    client.Name = HotelReservationDto.Name;
                    client.LastName = HotelReservationDto.LastName;
                    client.DateBirth = HotelReservationDto.DateBirth;
                    client.Gender = HotelReservationDto.Gender;
                    client.DocumentType = HotelReservationDto.DocumentType;
                    client.DocumentNumber = HotelReservationDto.DocumentNumber;
                    client.Email = HotelReservationDto.Email;
                    client.PhoneNumber = HotelReservationDto.PhoneNumber;

                    _context.Client.Add(client);
                    _context.SaveChanges();
                }

                EmergencyContact EmergencyContact = new EmergencyContact();

                EmergencyContact.CompleteName = HotelReservationDto.CompleteNameEmergencyContact;
                EmergencyContact.PhoneNumber = HotelReservationDto.PhoneNumberEmergencyContact;

                _context.EmergencyContact.Add(EmergencyContact);
                _context.SaveChanges();

                Reservation Reservation = new Reservation();

                Reservation.IdRoom = HotelReservationDto.IdRoom;
                Reservation.IdClient = client.IdClient;
                Reservation.IdEmergencyContact = EmergencyContact.IdEmergencyContact;
                Reservation.NumberOfPeople = HotelReservationDto.NumberOfPeople;
                Reservation.InitDate = HotelReservationDto.InitDate;
                Reservation.FinalDate = HotelReservationDto.FinalDate;
                Reservation.state = State.Active;

                _context.Reservation.Add(Reservation);
                _context.SaveChanges();

                return new Response<string>("OK", true, "Successful booking");

            }
            catch (Exception ex)
            {
                return new Response<string>("ERROR", false, "error, please contact IT");
            }
        }

        public List<Room> ValidateRoomAvailable(DateTime initDate, DateTime finalDate, int numberOfPeople, string City)
        {
            // validate if there are any reservations based on dates
            var reservation = _context.Reservation.Where(x =>
                    (x.InitDate <= initDate && initDate <= x.FinalDate) || (x.InitDate <= finalDate && finalDate <= x.FinalDate) || (initDate <= x.InitDate && x.FinalDate <= finalDate) && (x.state == State.Active)
                ).Select(x => x.IdRoom).ToArray();

            // get the hotels in the selected location
            var hotels = _context.Hotels.Where(x => x.Ubication.ToLower().Contains(City.ToLower())).Select(x=>x.IdHotel).ToArray();

            // validate rooms available according to capacity and if the room is active
            var rooms = _context.Rooms.Where(x => !reservation.Contains(x.IdRoom) && x.Capacity >= numberOfPeople && hotels.Contains(x.IdHotel) && (x.State == State.Active)).ToList();

            return rooms;
        }

        public Response<object> Reservations()
        {
            try
            {

                var queryReserves = from reserve in _context.Reservation
                        join room in _context.Rooms
                        on reserve.IdRoom equals room.IdRoom
                        join hotel in _context.Hotels
                        on room.IdHotel equals hotel.IdHotel
                        join client in _context.Client
                        on reserve.IdClient equals client.IdClient
                        join emergencyContact in _context.EmergencyContact
                        on reserve.IdEmergencyContact equals emergencyContact.IdEmergencyContact
                        where reserve.state == State.Active
                        select new {
                            Hotel = hotel.Name,
                            HotelUbication = hotel.Ubication,
                            RoomNumber = room.Number,
                            RoomType = room.RoomType,
                            RoomUbication = room.Ubication,
                            NameClient = $"{client.Name} {client.LastName}",
                            ClientDocumenType = client.DocumentType,
                            ClientDocument = client.DocumentNumber,
                            ClientEmail = client.Email,
                            ClientPhoneNumber = client.PhoneNumber,
                            NumberOfPeople = reserve.NumberOfPeople,
                            InitDate = reserve.InitDate,
                            FinalDate = reserve.FinalDate,
                            emergencyContactName = emergencyContact.CompleteName,
                            emergencyContactPhoneNumber = emergencyContact.PhoneNumber
                        };

                return new Response<object>(queryReserves.ToList(), true, "Reserves found");

            }
            catch
            {
                return new Response<object>(null, false, "error, please contact IT");
            }
        }
    }
}
