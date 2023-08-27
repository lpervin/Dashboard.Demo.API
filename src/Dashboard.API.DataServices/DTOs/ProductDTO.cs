namespace Dashboard.API.DataServices.DTOs;

public class ProductDTO
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public int ProductCategoryId { get; set; }
    public string? ProductCategory { get; set; }
    public decimal Price { get; set; }
}