using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using AutoMapper;
using CartItem = Ambev.DeveloperEvaluation.WebApi.Features.Cart.Common.CartItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;

/// <summary>
/// Profile for mapping between Application and API CreateCart responses
/// </summary>
public class CreateCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateCart feature
    /// </summary>
    public CreateCartProfile()
    {
        CreateMap<CartItem, CartItemCommand>();
        CreateMap<CreateCartRequest, CreateCartCommand>()
            .ForMember(dest => dest.Items, opt => 
                opt.MapFrom(src => src.Items));
        
        CreateMap<CreateCartResult, CreateCartResponse>();
    }
}