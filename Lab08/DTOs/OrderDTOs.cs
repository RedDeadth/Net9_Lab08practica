namespace Lab08.DTOs;

public class OrderDTOs
{
    // DTO para el detalle de productos en una orden (Ejercicio 3)
    public class OrderProductDetailDto
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

    // DTO para resumen de la orden
    public class OrderDetailSummaryDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ClientName { get; set; } = null!;
        public List<OrderProductDetailDto> Products { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }
    public class OrderQuantityDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalQuantity { get; set; }
    }
}