import { useState, useEffect } from "react";
import axiosInstance from "../../services/axiosInstance";

const SellerDashboardPage = () => {
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const res = await axiosInstance.get("/seller/dashboard");
        setStats(res.data);
      } catch (err) {
        console.error("Failed to load seller stats", err);
      } finally {
        setLoading(false);
      }
    };
    fetchStats();
  }, []);

  if (loading) return <div style={{ padding: "2rem", color: "#666" }}>Loading dashboard...</div>;
  if (!stats) return <div style={{ padding: "2rem", color: "#666" }}>Could not load stats.</div>;

  const cards = [
    { label: "Total Products", value: stats.totalProducts },
    { label: "Approved", value: stats.approvedProducts },
    { label: "Pending Approval", value: stats.pendingProducts },
    { label: "Rejected/Suspended", value: stats.rejectedProducts },
    { label: "Today's Orders", value: stats.todayOrders },
    { label: "Monthly Orders", value: stats.monthlyOrders },
    { label: "Revenue", value: `PKR ${stats.totalRevenue.toLocaleString()}` },
    { label: "Pending Shipments", value: stats.pendingShipments },
    {
      label: "Average Rating",
      value: stats.averageRating ? `${stats.averageRating.toFixed(1)} ★` : "N/A",
    },
  ];

  return (
    <div>
      <h2 className="section-title">Dashboard</h2>
      <div className="stats-grid">
        {cards.map((card) => (
          <div key={card.label} className="stat-card">
            <p className="stat-value">{card.value}</p>
            <p className="stat-label">{card.label}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default SellerDashboardPage;