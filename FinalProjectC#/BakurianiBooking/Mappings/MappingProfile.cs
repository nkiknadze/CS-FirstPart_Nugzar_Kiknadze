using AutoMapper;
using BakurianiBooking.Models.DTOs;
using BakurianiBooking.Models.Entities;

namespace BakurianiBooking.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookingCreateDto, Booking>();
            CreateMap<BookingUpdateDto, Booking>();
            CreateMap<Booking, BookingDetailsDto>();
            CreateMap<Room , RoomDetailsDto>();
            CreateMap<Room, RoomUpdateDto>();
            CreateMap<RoomUpdateDto, Room>();
            CreateMap<RoomCreateDto, Room>();
            CreateMap<Hotel, HotelDetailsDto>();
            CreateMap<HotelUpdateDto, Hotel>();
            CreateMap<HotelCreateDto, Hotel>();
            CreateMap<Hotel, HotelUpdateDto>();
            CreateMap<Booking, BookingUpdateDto>();

        }
    }
}