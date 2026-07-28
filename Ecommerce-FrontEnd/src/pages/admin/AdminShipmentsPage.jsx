import { useState, useEffect } from "react";
import axiosInstance from "../../services/axiosInstance";

const AdminShipmentsPage = () => {
  const [shipments, setShipments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [search, setSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState("");

  const fetchShipments = async () => {
    setLoading(true);
    try {
      const res = await axiosInstance.get("/admin/shipments", {
        params: { page, pageSize: 10, search, status: statusFilter },
      });
      setShipments(res.data.items || []);
      setTotalPages(res.data.totalPages);
    } catch (err) {
      console.error("Failed to load shipments", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchShipments(); }, [page, search, statusFilter]);

  return (
    <div>
      <h2 className="section-title">Shipments</h2>
      <div style={{ display: "flex", gap: "1rem", marginBottom: "1rem", flexWrap: "wrap" }}>
        <input className="form-input" placeholder="Search by tracking or carrier..." value={search}
          onChange={(e) => { setSearch(e.target.value); setPage(1); }} style={{ width: "250px" }} />
        <select className="form-input" style={{ width: "auto" }} value={statusFilter}
          onChange={(e) => { setStatusFilter(e.target.value); setPage(1); }}>
          <option value="">All Status</option>
          <option value="Pending">Pending</option>
          <option value="Packed">Packed</option>
          <option value="Dispatched">Dispatched</option>
          <option value="OutForDelivery">Out For Delivery</option>
          <option value="Delivered">Delivered</option>
        </select>
      </div>

      {loading ? <p style={{ color: "#666" }}>Loading...</p> :
        shipments.length === 0 ? <div className="empty-state">No shipments found.</div> :
        <table className="product-table">
          <thead><tr><th>Shipment ID</th><th>Order ID</th><th>Carrier</th><th>Tracking</th><th>Status</th><th>Created</th></tr></thead>
          <tbody>
            {shipments.map(s => (
              <tr key={s.id}>
                <td>{s.id.slice(0,8).toUpperCase()}</td>
                <td>{s.sellerOrderId?.slice(0,8).toUpperCase()}</td>
                <td>{s.carrier || "N/A"}</td>
                <td>{s.trackingNumber || "N/A"}</td>
                <td>{s.status}</td>
                <td>{new Date(s.createdAt).toLocaleDateString()}</td>
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

export default AdminShipmentsPage;