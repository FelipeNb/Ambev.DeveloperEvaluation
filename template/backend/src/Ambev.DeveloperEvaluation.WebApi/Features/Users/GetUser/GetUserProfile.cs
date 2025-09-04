using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

/// <summary>
/// Profile for mapping GetUser feature requests to commands
/// </summary>
public class GetUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUser feature
    /// </summary>
    public GetUserProfile()
    {
        CreateMap<GetUserRequest, GetUserCommand>();
        
        CreateMap<Guid, GetUserCommand>()
            .ConstructUsing(id => new GetUserCommand(id));
        
        // Do Result -> Response
        CreateMap<User, GetUserResponse>();
        CreateMap<GetUserResult, GetUserResponse>()
            .ForMember(dest => dest.Name, opt => 
                opt.MapFrom(src => new GetUserNameResponse
                {
                    FirstName = src.FirstName,
                    LastName = src.LastName
                }))
            .ForMember(dest => dest.Address, opt => 
                opt.MapFrom(src => new GetUserAddressResponse
                {
                    City = src.City,
                    Street = src.Street,
                    Number = src.Number,
                    Zipcode = src.Zipcode,
                    GeoLocation = new GetUserGeoLocationResponse
                    {
                        Lat = src.Latitude,
                        Long = src.Longitude
                    }
                }));
    }
}