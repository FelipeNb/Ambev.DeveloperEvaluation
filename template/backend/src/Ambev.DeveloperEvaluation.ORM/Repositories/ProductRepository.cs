using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories.Base;
using Ambev.DeveloperEvaluation.ORM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IProductRepository using Entity Framework Core
/// </summary>
public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    private new readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of ProductRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public ProductRepository(DefaultContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new product in the database
    /// </summary>
    /// <param name="product">The product to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created product</returns>
    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    /// <summary>
    /// Retrieves a product by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product if found, null otherwise</returns>
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products.FirstOrDefaultAsync(o=> o.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products
            .Where(x => x.Id == product.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.Title, product.Title)
                .SetProperty(x => x.Price, product.Price)
                .SetProperty(x => x.Description, product.Description)
                .SetProperty(x => x.Category, product.Category)
                .SetProperty(x => x.Image, product.Image)
                .SetProperty(x => x.RatingRate, product.RatingRate)
                .SetProperty(x => x.RatingCount, product.RatingCount),
                cancellationToken: cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves a product by their email address
    /// </summary>
    /// <param name="title"></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product if found, null otherwise</returns>
    public async Task<Product?> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(u => u.Title == title, cancellationToken);
    }

    /// <summary>
    /// Retrives all categories from products table
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<string>> GetAllCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .OrderBy(s => s.Category)
            .Select(s => s.Category)
            .ToListAsync();
    }

    public async Task<bool> ProductExistsAsync(Guid prodProductId)
    {
        return await _context.Products
            .AnyAsync(u => u.Id == prodProductId);
    }

    public async Task<decimal> GetPriceByIdAsync(Guid prodProductId)
    {
        return await _context.Products.AsNoTracking().Where(u => u.Id == prodProductId).Select(s => s.Price).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Deletes a product from the database
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the product was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(id, cancellationToken);
        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
    
    public async Task<PagedResult<Product>> GetAllByCategoryAsync(string category, int page, int size, string? order, CancellationToken ct)
    {
        var query = _context.Products.AsQueryable().Where(s => s.Category == category);
        query = ApplyOrdering(query, order);

        var totalItems = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * size).Take(size).ToListAsync(ct);

        return new PagedResult<Product>
        {
            Items = items,
            CurrentPage = page,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)size)
        };
    }

}
