using BakurianiBooking.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace BakurianiBooking.Models.DTOs
{
    public class BookingDetailsDto
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public string IdentityUserId { get; set; }
        public string Mobile { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public ICollection<RoomImage> RoomImages { get; set; }

    }
}