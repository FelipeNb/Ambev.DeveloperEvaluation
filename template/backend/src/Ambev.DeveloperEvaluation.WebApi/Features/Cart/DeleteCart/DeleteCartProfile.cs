using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.DeleteCart;

/// <summary>
/// Profile for mapping DeleteCart feature requests to commands
/// </summary>
public class DeleteCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for DeleteCart feature
    /// </summary>
    public DeleteCartProfile()
    {
        CreateMap<Guid, DeleteCartCommand>()
            .ConstructUsing(id => new DeleteCartCommand(id));
    }
}
