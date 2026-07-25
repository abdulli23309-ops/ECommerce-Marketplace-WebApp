namespace ECommerce.Application.DTOs.Admin
{
    public class AdminStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalSellers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingSellerApprovals { get; set; }
        public int PendingProductApprovals { get; set; }
        public int PendingReturns { get; set; }
    }
}