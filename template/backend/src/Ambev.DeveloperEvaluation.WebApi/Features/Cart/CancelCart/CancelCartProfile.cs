using Ambev.DeveloperEvaluation.Application.Carts.CancelCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CancelCart;

/// <summary>
/// Profile for mapping between Application and API CancelCart responses
/// </summary>
public class CancelCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelCart feature
    /// </summary>
    public CancelCartProfile()
    {
        CreateMap<CancelCartRequest, CancelCartCommand>();
        CreateMap<CancelCartResult, CancelCartResponse>();
    }
}