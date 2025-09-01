using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// Profile for mapping ListProducts feature requests to commands
/// </summary>
public class ListProductsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListProducts feature
    /// </summary>
    public ListProductsProfile()
    {
        CreateMap<Product, GetProductResponse>()
            .ForMember(dest => dest.Rating, opt => 
                opt.MapFrom(src => new RatingProduct
                {
                    Rate = src.RatingRate,
                    Count = src.RatingCount
                }));

        CreateMap<ListProductsResult, ListProductsResponse>()
            .ForMember(dest => dest.Items, opt => 
                opt.MapFrom(src => src.Items));
    }
}