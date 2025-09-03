using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Cart.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;

/// <summary>
/// Profile for mapping between Application and API GetCart responses
/// </summary>
public class GetCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetCart feature
    /// </summary>
    public GetCartProfile()
    {
        CreateMap<CartItem, CartItemCommand>();
        CreateMap<GetCartRequest, GetCartCommand>();
        
        CreateMap<GetCartResult, GetCartResponse>();
    }
}