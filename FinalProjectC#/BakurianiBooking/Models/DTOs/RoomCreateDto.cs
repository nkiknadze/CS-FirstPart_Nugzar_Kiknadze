using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakurianiBooking.Models.DTOs
{
    public class RoomCreateDto
    {
        [Required]
        public int HotelId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public List<IFormFile> ImageFiles { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
    }


}