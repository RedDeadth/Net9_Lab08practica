namespace Lab08.DTOs;

public class ProductDto
{
    public int Productid { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}