using BakurianiBooking.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakurianiBooking.Models.DTOs
{
    public class RoomDetailsDto
    {
        public int Id { get; set; }

        public int HotelId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public List<RoomImage> RoomImages { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }

        public DateTime? Created { get; set; }
    }
}