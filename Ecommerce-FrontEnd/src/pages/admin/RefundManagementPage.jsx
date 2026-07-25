import { useState, useEffect } from "react";
import { getReturns, getPayments, createRefund } from "../../services/adminService";

const RefundManagementPage = () => {
  const [approvedReturns, setApprovedReturns] = useState([]);
  const [payments, setPayments] = useState([]);
  const [formData, setFormData] = useState({
    paymentId: "",
    returnRequestId: "",
    amount: "",
  });
  const [message, setMessage] = useState(null);
  const [submitting, setSubmitting] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const load = async () => {
      try {
        const [returns, pmts] = await Promise.all([getReturns(), getPayments()]);
        setApprovedReturns(returns.filter((r) => r.status === "Approved"));
        setPayments(pmts);
      } catch (err) {
        console.error("Failed to load data", err);
      } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSubmitting(true);
    setMessage(null);

    try {
      const payload = {
        paymentId: formData.paymentId,
        returnRequestId: formData.returnRequestId,
        amount: parseFloat(formData.amount),
      };

      if (!payload.paymentId || !payload.returnRequestId || isNaN(payload.amount) || payload.amount <= 0) {
        setMessage({ type: "error", text: "Please fill all fields correctly." });
        setSubmitting(false);
        return;
      }

      await createRefund(payload);
      setMessage({ type: "success", text: "Refund created successfully!" });
      setFormData({ paymentId: "", returnRequestId: "", amount: "" });
    } catch (err) {
      console.error("Failed to create refund", err);
      setMessage({ type: "error", text: "Failed to create refund. Please check the details." });
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return <div style={{ padding: "2rem", color: "#666" }}>Loading...</div>;
  }

  return (
    <div>
      <h2 className="section-title">Refund Management</h2>

      {/* Refund Form */}
      <div style={{ background: "#fff", border: "1px solid #eaeaea", borderRadius: "0.5rem", padding: "2rem", marginBottom: "2rem", maxWidth: "600px" }}>
        <h3 style={{ fontWeight: 600, marginBottom: "1rem", color: "#000" }}>Create Refund</h3>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label className="form-label">Payment</label>
            <select
              name="paymentId"
              className="form-input"
              value={formData.paymentId}
              onChange={handleChange}
              required
            >
              <option value="">-- Select Payment --</option>
              {payments.map((p) => (
                <option key={p.paymentId} value={p.paymentId}>
                  Order #{p.orderId?.slice(0, 8)} – {p.customerEmail} – PKR {p.amount}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label className="form-label">Return Request</label>
            <select
              name="returnRequestId"
              className="form-input"
              value={formData.returnRequestId}
              onChange={handleChange}
              required
            >
              <option value="">-- Select Approved Return --</option>
              {approvedReturns.map((ret) => (
                <option key={ret.id} value={ret.id}>
                  {ret.productName} – {ret.customerEmail} – {ret.reason?.slice(0, 50)}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label className="form-label">Amount (PKR)</label>
            <input
              type="number"
              step="0.01"
              name="amount"
              className="form-input"
              value={formData.amount}
              onChange={handleChange}
              placeholder="e.g., 1999.98"
              required
            />
          </div>

          {message && (
            <p style={{
              color: message.type === "success" ? "#000" : "#d11a2a",
              fontSize: "0.875rem",
              fontWeight: 500,
              marginBottom: "1rem"
            }}>
              {message.text}
            </p>
          )}

          <button type="submit" className="btn-primary" disabled={submitting}>
            {submitting ? "Processing..." : "Create Refund"}
          </button>
        </form>
      </div>

      {/* Approved Returns (reference) */}
      <h3 style={{ fontWeight: 600, marginBottom: "1rem", color: "#000" }}>Approved Returns (Pending Refund)</h3>
      {approvedReturns.length === 0 ? (
        <p className="empty-state">No approved returns waiting for refund.</p>
      ) : (
        <table className="product-table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Customer</th>
              <th>Reason</th>
            </tr>
          </thead>
          <tbody>
            {approvedReturns.map((ret) => (
              <tr key={ret.id}>
                <td>{ret.productName}</td>
                <td>{ret.customerEmail}</td>
                <td>{ret.reason}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default RefundManagementPage;