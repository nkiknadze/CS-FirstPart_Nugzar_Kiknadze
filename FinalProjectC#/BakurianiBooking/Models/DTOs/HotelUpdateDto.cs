using System;
using System.ComponentModel.DataAnnotations;

namespace BakurianiBooking.Models.DTOs
{
    public class HotelUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Phone { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}