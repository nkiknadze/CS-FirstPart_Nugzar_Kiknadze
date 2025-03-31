namespace BakurianiBooking.Models.Entities
{
    public class Hotel
    {   public int Id {  get; set; }
        public string? City { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<HotelImage> HotelImages { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public string? OwnerEmail { get; set; }
    }
}
