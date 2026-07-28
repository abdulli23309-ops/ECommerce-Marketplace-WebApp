import { useState, useEffect } from "react";
import axiosInstance from "../../services/axiosInstance";

const AdminUsersPage = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [totalPages, setTotalPages] = useState(1);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [roleFilter, setRoleFilter] = useState("");
  const [activeFilter, setActiveFilter] = useState(""); // '', 'true', 'false'

  const fetchUsers = async () => {
    setLoading(true);
    try {
      const params = { page, pageSize: 10, search, sortBy: "newest" };
      if (roleFilter) params.role = roleFilter;
      if (activeFilter !== "") params.isActive = activeFilter === "true";
      const res = await axiosInstance.get("/admin/users", { params });
      setUsers(res.data.items || []);
      setTotalPages(res.data.totalPages);
    } catch (err) {
      console.error("Failed to load users", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, [page, search, roleFilter, activeFilter]);

  const handleToggleActive = async (userId, currentlyActive) => {
    const endpoint = currentlyActive ? "deactivate" : "activate";
    await axiosInstance.put(`/admin/users/${userId}/${endpoint}`);
    fetchUsers();
  };

  return (
    <div>
      <h2 className="section-title">User Management</h2>

      <div style={{ display: "flex", gap: "1rem", marginBottom: "1rem", flexWrap: "wrap" }}>
        <input className="form-input" placeholder="Search by name or email..." value={search}
          onChange={(e) => { setSearch(e.target.value); setPage(1); }} style={{ width: "250px" }} />
        <select className="form-input" style={{ width: "auto" }} value={roleFilter}
          onChange={(e) => { setRoleFilter(e.target.value); setPage(1); }}>
          <option value="">All Roles</option>
          <option value="Customer">Customer</option>
          <option value="Seller">Seller</option>
          <option value="SuperAdmin">SuperAdmin</option>
        </select>
        <select className="form-input" style={{ width: "auto" }} value={activeFilter}
          onChange={(e) => { setActiveFilter(e.target.value); setPage(1); }}>
          <option value="">All Status</option>
          <option value="true">Active</option>
          <option value="false">Inactive</option>
        </select>
      </div>

      {loading ? <p style={{ color: "#666" }}>Loading...</p> :
        users.length === 0 ? <div className="empty-state">No users found.</div> :
        <table className="product-table">
          <thead><tr><th>Name</th><th>Email</th><th>Roles</th><th>Status</th><th>Actions</th></tr></thead>
          <tbody>
            {users.map(user => (
              <tr key={user.id}>
                <td>{user.fullName}</td>
                <td>{user.email}</td>
                <td>{user.roles.join(", ")}</td>
                <td>{user.isActive ? "Active" : "Inactive"}</td>
                <td>
                  <button className="btn-edit" onClick={() => handleToggleActive(user.id, user.isActive)}>
                    {user.isActive ? "Deactivate" : "Activate"}
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      }

      {totalPages > 1 && (
        <div className="pagination">
          <button className="page-btn" disabled={page <= 1} onClick={() => setPage(page - 1)}>Previous</button>
          <span className="page-info">Page {page} of {totalPages}</span>
          <button className="page-btn" disabled={page >= totalPages} onClick={() => setPage(page + 1)}>Next</button>
        </div>
      )}
    </div>
  );
};

export default AdminUsersPage;