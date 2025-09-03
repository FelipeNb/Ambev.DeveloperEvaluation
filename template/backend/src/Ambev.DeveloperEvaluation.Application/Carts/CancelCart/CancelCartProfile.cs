using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.CancelCart;

/// <summary>
/// Profile for mapping between Cart entity and CancelCartResponse
/// </summary>
public class CancelCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelCart operation
    /// </summary>
    public CancelCartProfile()
    {
        CreateMap<CancelCartCommand, Cart>();
        CreateMap<Cart, CancelCartResult>();
    }
}
