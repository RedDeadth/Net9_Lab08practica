namespace Lab08.DTOs
{
    public class ClientDto
    {
        public int Clientid { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    public class ClientWithOrdersDto
    {
        public int Clientid { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<OrderSummaryDto> Orders { get; set; } = new();
    }

    public class OrderSummaryDto
    {
        public int Orderid { get; set; }
        public DateTime Orderdate { get; set; }
        public int TotalItems { get; set; }
    }
}