using HotelManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HotelManagement.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<EmergencyContact> EmergencyContact { get; set; }
        public DbSet<Reservation> Reservation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>(hotel =>
            {

                hotel.ToTable("Hotel");

                hotel.HasKey(c => c.IdHotel);

                hotel.Property(c => c.Name).IsRequired(true).HasMaxLength(150);

                hotel.Property(c => c.Address).IsRequired(true).HasMaxLength(100);

                hotel.Property(c => c.Phone).IsRequired(true).HasMaxLength(20);

                hotel.Property(c => c.Email).IsRequired(true).HasMaxLength(50);

                hotel.Property(c => c.comision).IsRequired(true);

                hotel.Property(c => c.Ubication).IsRequired(true).HasMaxLength(100);

                hotel.Property(c => c.State).IsRequired(true);

                hotel.HasMany(c => c.Rooms).WithOne(c => c.Hotel).HasForeignKey(c => c.IdHotel);

            }) ;

            modelBuilder.Entity<Room>(room => {

                room.ToTable("Room");

                room.HasKey(c => c.IdRoom);

                room.Property(c => c.IdHotel).IsRequired(true);

                room.Property(c => c.Number).IsRequired(true).HasMaxLength(20);

                room.Property(c => c.BaseCost).IsRequired(true);

                room.Property(c => c.Tax).IsRequired(true);

                room.Property(c => c.RoomType).IsRequired(true).HasMaxLength(50);

                room.Property(c => c.Ubication).IsRequired(true).HasMaxLength(100);

                room.Property(c => c.Capacity).IsRequired(true);

                room.Property(c => c.State).IsRequired(true);

                room.HasMany(c => c.Reservations).WithOne(c => c.Room).HasForeignKey(c => c.IdRoom);

            });

            modelBuilder.Entity<Customer>(Customer => {

                Customer.ToTable("Customer");

                Customer.HasKey(c => c.IdCustomer);

                Customer.Property(c => c.Name).IsRequired(true).HasMaxLength(50);

                Customer.Property(c => c.LastName).IsRequired(true).HasMaxLength(50);

                Customer.Property(c => c.DateBirth).IsRequired(true);

                Customer.Property(c => c.Gender).IsRequired(true);

                Customer.Property(c => c.DocumentType).IsRequired(true);

                Customer.Property(c => c.DocumentNumber).IsRequired(true);

                Customer.Property(c => c.Email).IsRequired(true).HasMaxLength(50);

                Customer.Property(c => c.PhoneNumber).IsRequired(true).HasMaxLength(20);

                Customer.HasOne(x => x.Reservations).WithMany(x => x.Customer).HasForeignKey(x => x.IdReservation);

            });


            modelBuilder.Entity<EmergencyContact>(emergencycontact => {

                emergencycontact.ToTable("EmergencyContact");

                emergencycontact.HasKey(c => c.IdEmergencyContact);

                emergencycontact.Property(c => c.CompleteName).IsRequired(true).HasMaxLength(100);

                emergencycontact.Property(c => c.PhoneNumber).IsRequired(true).HasMaxLength(20);

                emergencycontact.HasOne(x => x.Reservation).WithOne(x => x.EmergencyContact).HasForeignKey<Reservation>(x => x.IdEmergencyContact);

            });

            modelBuilder.Entity<Reservation>(reservation => {

                reservation.ToTable("Reservation");

                reservation.HasKey(c => c.IdReservation);

                reservation.Property(c => c.IdRoom).IsRequired(true);

                reservation.Property(c => c.InitDate).IsRequired(true);

                reservation.Property(c => c.FinalDate).IsRequired(true);

                reservation.Property(c => c.NumberOfPeople).IsRequired(true);

                reservation.Property(c => c.IdEmergencyContact).IsRequired(true);

                reservation.Property(c => c.state).IsRequired(true);

            });

        }
    }
}
