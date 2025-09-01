using Ambev.DeveloperEvaluation.Application.Products.ListProductCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProductCategory;

/// <summary>
/// Profile for mapping ListProductCategory feature requests to commands
/// </summary>
public class ListProductCategoryProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListProductCategory feature
    /// </summary>
    public ListProductCategoryProfile()
    {
        CreateMap<ListProductCategoryResult, ListProductCategoryResponse>();
    }
}