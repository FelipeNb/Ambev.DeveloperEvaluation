using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

/// <summary>
/// Profile for mapping GetUser feature requests to commands
/// </summary>
public class UpdateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUser feature
    /// </summary>
    public UpdateUserProfile()
    {
        CreateMap<UpdateUserRequest, UpdateUserCommand>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Address.Number))
            .ForMember(dest => dest.Zipcode, opt => opt.MapFrom(src => src.Address.Zipcode))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Address.GeoLocation.Lat))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Address.GeoLocation.Long));
        
        CreateMap<UpdateUserResult, UpdateUserResponse>()
            .ForMember(dest => dest.Name, opt => 
                opt.MapFrom(src => new UpdateUserNameResponse
                {
                    FirstName = src.FirstName,
                    LastName = src.LastName
                }))
            .ForMember(dest => dest.Address, opt => 
                opt.MapFrom(src => new UpdateUserAddressResponse
                {
                    City = src.City,
                    Street = src.Street,
                    Number = src.Number,
                    Zipcode = src.Zipcode,
                    GeoLocation = new UpdateUserGeoLocationResponse
                    {
                        Lat = src.Latitude,
                        Long = src.Longitude
                    }
                }));
    }
}