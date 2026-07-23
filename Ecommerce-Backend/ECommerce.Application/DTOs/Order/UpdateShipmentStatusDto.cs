namespace ECommerce.Application.DTOs.Orders
{
    public class UpdateShipmentStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public string? Location { get; set; }
    }
}