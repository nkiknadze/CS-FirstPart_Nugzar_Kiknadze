using System.ComponentModel.DataAnnotations.Schema;

namespace BakurianiBooking.Models.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public Hotel Hotel { get; set; }
        public int HotelId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl {  get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public ICollection<RoomImage> RoomImages { get; set; }
    }
}
