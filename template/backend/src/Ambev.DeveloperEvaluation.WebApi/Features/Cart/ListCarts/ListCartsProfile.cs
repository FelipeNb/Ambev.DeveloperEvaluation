using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.ListCarts;

/// <summary>
/// Profile for mapping ListCarts feature requests to commands
/// </summary>
public class ListCartsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListCarts feature
    /// </summary>
    public ListCartsProfile()
    {
        CreateMap<Domain.Entities.Cart, GetCartResult>();
        CreateMap<CartItem, CartItemCommand>();
        CreateMap<GetCartRequest, GetCartCommand>();
        
        CreateMap<Domain.Entities.Cart, GetCartResponse>();
        CreateMap<GetCartResult, GetCartResponse>();
        CreateMap<ListCartsResult, ListCartsResponse>()
            .ForMember(dest => dest.Items, opt => 
                opt.MapFrom(src => src.Items));
    }
}