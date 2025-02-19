using HotelManagement.Application.Data;
using HotelManagement.Application.Dtos;
using HotelManagement.Application.Services;
using HotelManagement.Domain.Enums;
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

                //Validate if the number of companions matches the number of companions you are trying to register
                if (HotelReservationDto.NumberOfPeople  != HotelReservationDto.Customer.Count)
                {
                    return new Response<string>("ERROR", false, "The number of persons does not match with the registered companions");
                }

                //Validate if more than one client is the owner
                if (HotelReservationDto.Customer.Count(x=> x.CustomerType == CustomerType.Owner) > 1)
                {
                    return new Response<string>("ERROR", false, "Only one client can be registered as owner");
                }
                
                // create emergency contact
                EmergencyContact EmergencyContact = new EmergencyContact();

                EmergencyContact.CompleteName = HotelReservationDto.EmergencyContact.CompleteName;
                EmergencyContact.PhoneNumber = HotelReservationDto.EmergencyContact.PhoneNumber;

                _context.EmergencyContact.Add(EmergencyContact);
                _context.SaveChanges();

                //create reservation
                Reservation Reservation = new Reservation();

                Reservation.IdRoom = HotelReservationDto.IdRoom;
                Reservation.IdEmergencyContact = EmergencyContact.IdEmergencyContact;
                Reservation.NumberOfPeople = HotelReservationDto.NumberOfPeople;
                Reservation.InitDate = HotelReservationDto.InitDate;
                Reservation.FinalDate = HotelReservationDto.FinalDate;
                Reservation.state = State.Active;

                _context.Reservation.Add(Reservation);
                _context.SaveChanges();

                //create customers
                foreach (var customer in HotelReservationDto.Customer)
                {
                    Customer CustomerNew = new Customer();

                    CustomerNew.IdReservation = Reservation.IdReservation;
                    CustomerNew.Name = customer.Name;
                    CustomerNew.LastName = customer.LastName;
                    CustomerNew.DateBirth = customer.DateBirth;
                    CustomerNew.Gender = customer.Gender;
                    CustomerNew.DocumentType = customer.DocumentType;
                    CustomerNew.DocumentNumber = customer.DocumentNumber;
                    CustomerNew.Email = customer.Email;
                    CustomerNew.PhoneNumber = customer.PhoneNumber;
                    CustomerNew.CustomerType = customer.CustomerType;

                    _context.Customer.Add(CustomerNew);
                    _context.SaveChanges();
                }

                return new Response<string>("OK", true, "Successful booking");

            }
            catch (Exception ex)
            {
                return new Response<string>("ERROR", false, "error, please contact IT");
            }
        }

        public Response<object> Reservations()
        {
            try
            {
                List<Dictionary<string, object>> Reserves = new List<Dictionary<string, object>>();

                var queryReserves = from reserve in _context.Reservation
                        join room in _context.Rooms
                        on reserve.IdRoom equals room.IdRoom
                        join hotel in _context.Hotels
                        on room.IdHotel equals hotel.IdHotel
                        join emergencyContact in _context.EmergencyContact
                        on reserve.IdEmergencyContact equals emergencyContact.IdEmergencyContact
                        where reserve.state == State.Active && hotel.State == State.Active && room.State == State.Active
                        select new {
                            IdReservation = reserve.IdReservation,
                            Hotel = hotel.Name,
                            HotelUbication = hotel.Ubication,
                            RoomNumber = room.Number,
                            RoomType = room.RoomType,
                            RoomUbication = room.Ubication,
                            NumberOfPeople = reserve.NumberOfPeople,
                            InitDate = reserve.InitDate,
                            FinalDate = reserve.FinalDate,
                            emergencyContactName = emergencyContact.CompleteName,
                            emergencyContactPhoneNumber = emergencyContact.PhoneNumber
                        };

                foreach(var reservation in queryReserves.ToList())
                {
                    var customerOwner = _context.Customer.First(x => x.IdReservation == reservation.IdReservation && x.CustomerType == CustomerType.Owner);
                    var customerCompanions = _context.Customer.Where(x => x.IdReservation == reservation.IdReservation && x.CustomerType == CustomerType.companion).ToList();

                    Dictionary<string, object> Reserve = new Dictionary<string, object>();

                    Reserve.Add("Hotel", reservation.Hotel);
                    Reserve.Add("HotelUbication", reservation.HotelUbication);
                    Reserve.Add("RoomNumber", reservation.RoomNumber);
                    Reserve.Add("RoomType", reservation.RoomType);
                    Reserve.Add("RoomUbication", reservation.RoomUbication);
                    Reserve.Add("customerOwner", customerOwner);
                    Reserve.Add("customerCompanions", customerCompanions);
                    Reserve.Add("NumberOfPeople", reservation.NumberOfPeople);
                    Reserve.Add("InitDate", reservation.InitDate);
                    Reserve.Add("FinalDate", reservation.FinalDate);
                    Reserve.Add("emergencyContactName", reservation.emergencyContactName);
                    Reserve.Add("emergencyContactPhoneNumber", reservation.emergencyContactPhoneNumber);

                    Reserves.Add(Reserve);
                }

                if(Reserves.Count == 0)
                {
                    return new Response<object>(null, true, "There are no active reservations");
                }

                return new Response<object>(Reserves, true, "Reserves found");
               
            }
            catch
            {
                return new Response<object>(null, false, "error, please contact IT");
            }
        }

        public Response<string> UpdateStatusReservation(ReservationStatusDto ReservationStatusDto)
        {
            try
            {
                if (ReservationStatusDto.IdReservation == null || ReservationStatusDto.IdReservation == 0)
                {
                    return new Response<string>("ERROR", false, "Reservation id must be sent");
                };

                var reservation = _context.Reservation.FirstOrDefault(x => x.IdReservation == ReservationStatusDto.IdReservation);

                if (reservation == null)
                {
                    return new Response<string>("ERROR", false, "The reservation must be registered");
                }
                reservation.state = ReservationStatusDto.State;
                _context.Reservation.Update(reservation);
                _context.SaveChanges();

                return new Response<string>("OK", true, "Reservation succesfully updated");
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

    }
}
