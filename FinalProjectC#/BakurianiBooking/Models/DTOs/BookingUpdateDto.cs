using System;
using System.ComponentModel.DataAnnotations;

namespace BakurianiBooking.Models.DTOs
{
    public class BookingUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int HotelId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public string IdentityUserId { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Status { get; set; }
    }
}