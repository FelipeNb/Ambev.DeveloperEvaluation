using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

/// <summary>
/// Profile for mapping GetProduct feature requests to commands
/// </summary>
public class GetProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetProduct feature
    /// </summary>
    public GetProductProfile()
    {
        CreateMap<GetProductRequest, GetProductCommand>();
        
        CreateMap<Guid, GetProductCommand>()
            .ConstructUsing(id => new GetProductCommand(id));
        
        // Do Result -> Response
        CreateMap<GetProductResult, GetProductResponse>()
            .ForMember(dest => dest.Rating, opt => 
                opt.MapFrom(src => new RatingProduct()
                {
                    Rate = src.RatingRate,
                    Count = src.RatingCount
                }));
    }
}