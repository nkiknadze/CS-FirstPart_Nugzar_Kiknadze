using System;

namespace BakurianiBooking.Models.DTOs
{
    public class HotelDetailsDto
    {
        public int Id { get; set; }

        public string City { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Phone { get; set; }

        public string ImageUrl { get; set; }

        public string? OwnerEmail { get; set; }

        public DateTime? Created { get; set; }
    }
}