using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Base;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductCategory;

/// <summary>
/// Profile for mapping between Product entity and GetProductCategoryResponse
/// </summary>
public class GetProductCategoryProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetProductCategory operation
    /// </summary>
    public GetProductCategoryProfile()
    {
        CreateMap<PagedResult<Product>, GetProductsCategoryResult>();
    }
}
