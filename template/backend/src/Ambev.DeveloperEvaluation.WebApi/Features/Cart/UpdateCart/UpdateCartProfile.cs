using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using AutoMapper;
using CartItem = Ambev.DeveloperEvaluation.WebApi.Features.Cart.Common.CartItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart;

/// <summary>
/// Profile for mapping between Application and API UpdateCart responses
/// </summary>
public class UpdateCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateCart feature
    /// </summary>
    public UpdateCartProfile()
    {
        CreateMap<CartItem, CartItemCommand>();
        CreateMap<UpdateCartRequest, UpdateCartCommand>()
            .ForMember(dest => dest.Items, opt => 
                opt.MapFrom(src => src.Items));
        
        CreateMap<UpdateCartResult, UpdateCartResponse>();
    }
}