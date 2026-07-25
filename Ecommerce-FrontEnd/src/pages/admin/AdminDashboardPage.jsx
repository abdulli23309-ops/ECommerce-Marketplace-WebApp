import { useState, useEffect } from "react";
import { getAdminStats } from "../../services/adminService";

const AdminDashboardPage = () => {
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getAdminStats()
      .then(data => setStats(data))
      .catch(err => console.error(err))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <div style={{ padding: "2rem", color: "#666" }}>Loading stats...</div>;
  if (!stats) return <div style={{ padding: "2rem", color: "#666" }}>Failed to load stats.</div>;

  const cards = [
    { label: "Total Users", value: stats.totalUsers },
    { label: "Sellers", value: stats.totalSellers },
    { label: "Products", value: stats.totalProducts },
    { label: "Orders", value: stats.totalOrders },
    { label: "Revenue", value: `PKR ${stats.totalRevenue?.toLocaleString()}` },
    { label: "Pending Sellers", value: stats.pendingSellerApprovals, highlight: stats.pendingSellerApprovals > 0 },
    { label: "Pending Products", value: stats.pendingProductApprovals, highlight: stats.pendingProductApprovals > 0 },
    { label: "Pending Returns", value: stats.pendingReturns, highlight: stats.pendingReturns > 0 },
  ];

  return (
    <div>
      <h2 className="section-title">Dashboard</h2>
      <div className="stats-grid">
        {cards.map((card) => (
          <div
            key={card.label}
            className={`stat-card ${card.highlight ? "stat-card-highlight" : ""}`}
          >
            <p className="stat-value">{card.value}</p>
            <p className="stat-label">{card.label}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default AdminDashboardPage;