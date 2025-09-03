using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories.Base;
using Ambev.DeveloperEvaluation.ORM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ICartRepository using Entity Framework Core
/// </summary>
public class CartRepository : RepositoryBase<Cart>, ICartRepository
{
    private new readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of CartRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public CartRepository(DefaultContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new cart in the database
    /// </summary>
    /// <param name="cart">The cart to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart</returns>
    public async Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        await _context.Carts.AddAsync(cart, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cart;
    }
    
    /// <summary>
    /// Retrieves a cart by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the cart</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart if found, null otherwise</returns>
    public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Carts
            .Include(s => s.Items)
            .FirstOrDefaultAsync(o=> o.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        var existingCart = await _context.Carts
                               .FirstOrDefaultAsync(c => c.Id == cart.Id, cancellationToken)
                           ?? throw new InvalidOperationException("Cart not found");

        // removes all associated products 
        _context.CartProducts.RemoveRange(
            _context.CartProducts.Where(cp => cp.CartId == existingCart.Id)
        );

        existingCart.Branch = cart.Branch;
        existingCart.UserId = cart.UserId;

        existingCart.Items = cart.Items.Select(i => new CartProduct
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            CartId = cart.Id
        }).ToList();

        await _context.SaveChangesAsync(cancellationToken);


    }
    
    /// <summary>
    /// Deletes a cart from the database
    /// </summary>
    /// <param name="id">The unique identifier of the cart to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the cart was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cart = await GetByIdAsync(id, cancellationToken);
        if (cart == null)
            return false;

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task CancelAsync(Guid cartId, CancellationToken cancellationToken)
    {
        await _context.Carts
            .Where(x => x.Id == cartId)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Cancelled, true),
                cancellationToken: cancellationToken);
    }


    public async Task<long> NextSaleSequenceAsync()
    {
        var maxSaleNumber = await _context.Carts
            .MaxAsync(c => (long?)c.SaleNumber) ?? 0;

        return maxSaleNumber + 1;
    }
    
    public override async Task<PagedResult<Cart>> GetAllAsync(int page, int size, string? order, CancellationToken ct)
    {
        var query = _context.Carts
            .Include(s => s.Items)
            .AsQueryable();
        
        query = ApplyOrdering(query, order);

        var totalItems = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * size).Take(size).ToListAsync(ct);

        return new PagedResult<Cart>
        {
            Items = items,
            CurrentPage = page,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)size)
        };
    }

}