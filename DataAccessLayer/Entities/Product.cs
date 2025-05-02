using System.ComponentModel.DataAnnotations;

namespace eCommerce.DataAccessLayer.Entities;

public class Product
{
    [Key]
    public Guid ProductID { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public double? UnitPrice { get; set; }

    public int? QuantityInStock { get; set; }
}