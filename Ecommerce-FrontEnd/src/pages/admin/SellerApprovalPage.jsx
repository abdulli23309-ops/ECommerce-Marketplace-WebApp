import { useState, useEffect } from "react";
import { getSellers, approveSeller, rejectSeller } from "../../services/adminService";

const SellerApprovalPage = () => {
  const [sellers, setSellers] = useState([]);
  const [loading, setLoading] = useState(true);

  const loadSellers = async () => {
    setLoading(true);
    try {
      const data = await getSellers();
      setSellers(data || []);
    } catch (err) {
      console.error("Failed to load sellers", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadSellers();
  }, []);

  const handleAction = async (sellerId, action) => {
    try {
      if (action === "approve") await approveSeller(sellerId);
      else await rejectSeller(sellerId);
      // Refresh the list
      await loadSellers();
    } catch (err) {
      console.error(`Failed to ${action} seller`, err);
    }
  };

  if (loading) {
    return <div style={{ padding: "2rem", color: "#666" }}>Loading sellers...</div>;
  }

  return (
    <div>
      <h2 className="section-title">Seller Approval</h2>

      {sellers.length === 0 ? (
        <div className="empty-state">No sellers to review.</div>
      ) : (
        <table className="product-table">
          <thead>
            <tr>
              <th>Business Name</th>
              <th>Owner</th>
              <th>Email</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {sellers.map((seller) => (
              <tr key={seller.id}>
                <td>{seller.businessName}</td>
                <td>{seller.fullName}</td>
                <td>{seller.email}</td>
                <td>
                  <span
                    style={{
                      fontWeight: 600,
                      color: seller.status === "Approved" ? "#000" : "#666",
                    }}
                  >
                    {seller.status}
                  </span>
                </td>
                <td>
                  {seller.status === "Pending" && (
                    <>
                      <button
                        className="btn-edit"
                        onClick={() => handleAction(seller.id, "approve")}
                      >
                        Approve
                      </button>
                      <button
                        className="btn-delete"
                        onClick={() => handleAction(seller.id, "reject")}
                      >
                        Reject
                      </button>
                    </>
                  )}
                  {seller.status !== "Pending" && (
                    <span style={{ color: "#999" }}>—</span>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default SellerApprovalPage;