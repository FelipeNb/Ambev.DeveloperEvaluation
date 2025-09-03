using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Base;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

/// <summary>
/// Profile for mapping between Product entity and ListProductsResponse
/// </summary>
public class ListProductsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListProducts operation
    /// </summary>
    public ListProductsProfile()
    {
        CreateMap<Product, ListProductsResult>();
        CreateMap<PagedResult<Product>, ListProductsResult>();
        
        
    }
}
