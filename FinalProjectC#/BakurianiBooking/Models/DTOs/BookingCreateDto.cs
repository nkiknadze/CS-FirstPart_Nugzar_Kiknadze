using System;
using System.ComponentModel.DataAnnotations;

namespace BakurianiBooking.Models.DTOs
{
    public class BookingCreateDto
    {
        [Required]
        public int HotelId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public string IdentityUserId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 9)]
        public string Mobile { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [GreaterThan("StartDate", ErrorMessage = "დასრულების თარიღი უნდა იყოს საწყის თარიღზე დიდი.")]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "ფასი უნდა იყოს დადებითი რიცხვი.")]
        public decimal Price { get; set; }

        public string? Status { get; set; }

        public string? PaymentStatus { get; set; }
    }

    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public GreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateTime)value;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
            {
                return new ValidationResult($"Property {_comparisonProperty} not found.");
            }

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);
            if (currentValue <= comparisonValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}