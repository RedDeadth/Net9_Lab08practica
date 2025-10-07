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

   
    public class ClientWithOrderCountDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public int OrderCount { get; set; }
    }

    public class ClientProductsDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int ProductCount { get; set; }
        public List<string> Products { get; set; }
    }
}