namespace Lab08.DTOs;

public class OrderDetailDto
{
    public int OrderId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}
public class OrderSummaryDto
{
    public int Orderid { get; set; }
    public DateTime Orderdate { get; set; }
    public int TotalItems { get; set; }
}