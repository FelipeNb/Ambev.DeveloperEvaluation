using System.Text.Json.Serialization;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart : BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Username => $"{User.FirstName} {User.LastName}";
    public DateTime? Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long SaleNumber { get; set; }
    public string Branch { get; set; } = string.Empty;
    public bool Cancelled { get; private set; }
    public List<CartProduct> Items { get; set; } = new();

    public decimal TotalAmount
    {
        get => Math.Round(Items.Sum(i => i.Total), 2);
        set { }
    }

    public void Cancel()
    {
        if (Cancelled) return;
        Cancelled = true;
    }

    public void AddOrUpdateItem(Guid productId, int quantity, decimal unitPrice)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
        {
            item = new CartProduct
            {
                Id = Guid.NewGuid(),
                CartId = this.Id,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = unitPrice,
            };
            Items.Add(item);
        }
        else
        {
            item.Quantity = quantity;
            item.UnitPrice = unitPrice;
        }
    }

    public void RemoveItem(Guid productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null) return;
        Items.Remove(item);
    }

    public void ChangeUser(Guid userId)
    {
        UserId = userId;
    }

    public Cart()
    {
        CreatedAt = DateTime.UtcNow;
        Date = DateTime.UtcNow;
    }

    public ValidationResultDetail Validate()
    {
        var validator = new CartValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    // Navigation properties
    [JsonIgnore] 
    public virtual User User { get; set; } = null!;
}