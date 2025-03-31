using System;
using System.ComponentModel.DataAnnotations;

namespace BakurianiBooking.Models.DTOs
{
    public class HotelCreateDto
    {
        [Required]
        public string City { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Phone { get; set; }

        public IFormFile? ImageFile { get; set; }

        public string? OwnerEmail { get; set; }
    }
}