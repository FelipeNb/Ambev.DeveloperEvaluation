using Ambev.DeveloperEvaluation.Application.Products.GetProductCategory;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductsCategory;

/// <summary>
/// Profile for mapping GetProductCategory feature requests to commands
/// </summary>
public class GetProductCategoryProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetProductCategory feature
    /// </summary>
    public GetProductCategoryProfile()
    {
        CreateMap<Product, GetProductResponse>()
            .ForMember(dest => dest.Rating, opt => 
                opt.MapFrom(src => new RatingProduct
                {
                    Rate = src.RatingRate,
                    Count = src.RatingCount
                }));

        CreateMap<GetProductsCategoryResult, GetProductCategoryResponse>()
            .ForMember(dest => dest.Items, opt => 
                opt.MapFrom(src => src.Items));
    }
}