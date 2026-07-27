namespace ECommerce.Application.DTOs.Seller
{
    public class SellerDashboardDto
    {
        public int TotalProducts { get; set; }
        public int ApprovedProducts { get; set; }
        public int PendingProducts { get; set; }
        public int RejectedProducts { get; set; }
        public int TodayOrders { get; set; }
        public int MonthlyOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingShipments { get; set; }
        public double? AverageRating { get; set; }
    }
}