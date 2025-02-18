using HotelManagement.Application.Data;
using HotelManagement.Domain.Interfaces;
using HotelManagement.Domain.Models;
using static HotelManagement.Application.Dtos.HotelManagementDto;

namespace HotelManagement.Infraestructure.Repositories
{
    public class HotelManagementRepository : IHotelManagement
    {
        private readonly ApplicationDbContext _context;
        public HotelManagementRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Response<int> CreateHotel(Hotel HotelData)
        {
            try
            {
                _context.Hotels.Add(HotelData);
                _context.SaveChanges();

                return new Response<int>(HotelData.IdHotel, true, "Hotel succesfully created");
            }
            catch (Exception ex) 
            { 
                return new Response<int>(0, false, "error when creating hotel, please contact IT");
            }
        }

        public Response<Hotel> UpdateHotel(Hotel HotelData)
        {
            try
            {
                if(HotelData.IdHotel == null || HotelData.IdHotel == 0)
                {
                    return new Response<Hotel>(null, false, "Hotel id must be sent");
                };

                if(_context.Hotels.Count(x => x.IdHotel == HotelData.IdHotel) == 0 )
                {
                    return new Response<Hotel>(null, false, "The Hotel must be registered");
                }

                _context.Hotels.Update(HotelData);
                _context.SaveChanges();

                return new Response<Hotel>(HotelData, true, "Hotel succesfully updated");
            }
            catch (Exception ex)
            {
                return new Response<Hotel>(null, false, "error when updating hotel, please contact IT");
            }
        }

        public Response<string> UpdateStatusHotel(HotelDto HotelDto)
        {
            try
            {
                if (HotelDto.IdHotel == null || HotelDto.IdHotel == 0)
                {
                    return new Response<string>("ERROR", false, "Hotel id must be sent");
                };

                var Hotel = _context.Hotels.FirstOrDefault(x=> x.IdHotel == HotelDto.IdHotel);

                if(Hotel == null)
                {
                    return new Response<string>("ERROR", false, "The hotel must be registered");
                }
                Hotel.State = HotelDto.State;
                _context.Hotels.Update(Hotel);
                _context.SaveChanges();

                return new Response<string>("OK", true, "Hotel succesfully updated");
            }
            catch (Exception ex)
            {
                return new Response<string>("ERROR", false, "error when creating hotel, please contact IT");
            }
        }

        public Response<int> CreateRoom(Room RoomData)
        {
            try
            {
                if (_context.Hotels.Count(x=> x.IdHotel == RoomData.IdHotel) == 0)
                {
                    return new Response<int>(0, false, "error when creating Room, The Hotel must be registered");
                }
                _context.Rooms.Add(RoomData);
                _context.SaveChanges();

                return new Response<int>(RoomData.IdRoom, true, "Room succesfully created");
            }
            catch (Exception ex)
            {
                return new Response<int>(0, false, "error when creating Room, please contact IT");
            }
        }

        public Response<Room> UpdateRoom(Room RoomData)
        {
            try
            {
                if (RoomData.IdRoom == null || RoomData.IdRoom == 0)
                {
                    return new Response<Room>(null, false, "Room id must be sent");
                };

                if (_context.Rooms.Count(x => x.IdRoom == RoomData.IdRoom) == 0)
                {
                    return new Response<Room>(null, false, "The Room must be registered");
                }

                if (_context.Hotels.Count(x => x.IdHotel == RoomData.IdHotel) == 0)
                {
                    return new Response<Room>(null, false, "error when updating Room, The Hotel must be registered");
                }

                _context.Rooms.Update(RoomData);
                _context.SaveChanges();

                return new Response<Room>(RoomData, true, "Room succesfully updated");
            }
            catch (Exception ex)
            {
                return new Response<Room>(null, false, "error when updating Room, please contact IT");
            }
        }

        public Response<string> UpdateStatusRoom(RoomDto RoomDto)
        {
            try
            {
                if (RoomDto.IdRoom == null || RoomDto.IdRoom == 0)
                {
                    return new Response<string>("ERROR", false, "Room id must be sent");
                };

                var Room = _context.Rooms.FirstOrDefault(x => x.IdRoom == RoomDto.IdRoom);

                if (Room == null)
                {
                    return new Response<string>("ERROR", false, "The Room must be registered");
                }
                Room.State = RoomDto.State;
                _context.Rooms.Update(Room);
                _context.SaveChanges();

                return new Response<string>("OK", true, "Room succesfully updated");
            }
            catch (Exception ex)
            {
                return new Response<string>("ERROR", false, "error when creating Room, please contact IT");
            }
        }
    }
}
