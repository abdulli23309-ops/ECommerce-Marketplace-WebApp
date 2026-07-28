import { useState, useEffect } from "react";
import axiosInstance from "../../services/axiosInstance";

const AdminOrdersPage = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [search, setSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState("");
  const [sort, setSort] = useState("newest");

  const fetchOrders = async () => {
    setLoading(true);
    try {
      const res = await axiosInstance.get("/admin/orders", {
        params: { page, pageSize: 10, search, status: statusFilter, sortBy: sort },
      });
      setOrders(res.data.items || []);
      setTotalPages(res.data.totalPages);
    } catch (err) {
      console.error("Failed to load orders", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchOrders(); }, [page, search, statusFilter, sort]);

  return (
    <div>
      <h2 className="section-title">Orders</h2>
      <div style={{ display: "flex", gap: "1rem", marginBottom: "1rem", flexWrap: "wrap" }}>
        <input className="form-input" placeholder="Search by ID or customer email..." value={search}
          onChange={(e) => { setSearch(e.target.value); setPage(1); }} style={{ width: "250px" }} />
        <select className="form-input" style={{ width: "auto" }} value={statusFilter}
          onChange={(e) => { setStatusFilter(e.target.value); setPage(1); }}>
          <option value="">All Status</option>
          <option value="Pending">Pending</option>
          <option value="Processing">Processing</option>
          <option value="Shipped">Shipped</option>
          <option value="Delivered">Delivered</option>
          <option value="Canceled">Canceled</option>
        </select>
        <select className="form-input" style={{ width: "auto" }} value={sort}
          onChange={(e) => { setSort(e.target.value); setPage(1); }}>
          <option value="newest">Newest First</option>
          <option value="oldest">Oldest First</option>
        </select>
      </div>

      {loading ? <p style={{ color: "#666" }}>Loading...</p> :
        orders.length === 0 ? <div className="empty-state">No orders found.</div> :
        <table className="product-table">
          <thead><tr><th>Order ID</th><th>Customer</th><th>Date</th><th>Total</th><th>Status</th></tr></thead>
          <tbody>
            {orders.map(order => (
              <tr key={order.id}>
                <td>{order.id.slice(0,8).toUpperCase()}</td>
                <td>{order.customerEmail || "N/A"}</td>
                <td>{new Date(order.orderDate).toLocaleDateString()}</td>
                <td>PKR {order.totalAmount?.toLocaleString()}</td>
                <td>{order.orderStatus}</td>
              </tr>
            ))}
          </tbody>
        </table>
      }

      {totalPages > 1 && (
        <div className="pagination">
          <button className="page-btn" disabled={page <= 1} onClick={() => setPage(page - 1)}>Previous</button>
          <span>Page {page} of {totalPages}</span>
          <button className="page-btn" disabled={page >= totalPages} onClick={() => setPage(page + 1)}>Next</button>
        </div>
      )}
    </div>
  );
};

export default AdminOrdersPage;