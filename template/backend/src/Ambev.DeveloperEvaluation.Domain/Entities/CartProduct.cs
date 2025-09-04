using System.Text.Json.Serialization;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class CartProduct : BaseEntity
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName => Product.Title;
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }

    public decimal DiscountPercent
    {
        get
        {
            return Quantity switch
            {
                // Regras: 1-3 => 0%, 4-9 => 10%, 10-20 => 20%; máximo 20 unidades
                >= 10 => 0.20m,
                >= 4 => 0.10m,
                _ => 0.00m
            };
        }
        set { }
    }

    public decimal Total
    {
        get
        {
            var effectiveQuantity = Math.Min(Quantity, 20); // respeitando máximo de 20
            var gross = UnitPrice * effectiveQuantity;
            var discount = gross * DiscountPercent;
            return decimal.Round(gross - discount, 2);
        }
        set{}
    }
    
    // Navigation properties
    [JsonIgnore] 
    public virtual Cart Cart { get; set; } = null!;
    
    [JsonIgnore] 
    public virtual Product Product { get; set; } = null!;
}