namespace Lab08.DTOs;

public class ProductDto
{
    public int Productid { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    
    public string ProductName { get; set; }
    
    public int ClientCount { get; set; }
    
    public List<string> Clients { get; set; }
    
}
public class ProductClientsDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int ClientCount { get; set; }
    public List<string> Clients { get; set; }
}