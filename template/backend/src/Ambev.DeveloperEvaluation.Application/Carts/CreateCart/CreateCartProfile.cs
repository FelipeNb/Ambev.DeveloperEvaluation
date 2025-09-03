using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Profile for mapping between Cart entity and CreateCartResponse
/// </summary>
public class CreateCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateCart operation
    /// </summary>
    public CreateCartProfile()
    {
        CreateMap<CartItemCommand, CartProduct>();
        CreateMap<CreateCartCommand, Cart>()
            .ForMember(dest => dest.Items,
                opt => opt.MapFrom(src => src.Items));

        CreateMap<Cart, CreateCartResult>();
    }
}