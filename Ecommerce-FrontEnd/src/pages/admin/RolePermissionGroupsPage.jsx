import { useState, useEffect } from "react";
import axiosInstance from "../../services/axiosInstance";

const RolePermissionGroupsPage = () => {
  const [roles, setRoles] = useState([]);
  const [groups, setGroups] = useState([]);
  const [selectedRoleId, setSelectedRoleId] = useState("");
  const [selectedGroupId, setSelectedGroupId] = useState("");
  const [assignments, setAssignments] = useState([]);

  useEffect(() => {
    axiosInstance.get("/admin/roles")
      .then(res => setRoles(res.data || []))
      .catch(console.error);
    axiosInstance.get("/admin/permission-groups?pageSize=100")
      .then(res => setGroups(res.data.items || []))
      .catch(console.error);
  }, []);

  const fetchAssignments = async (roleId) => {
    if (!roleId) {
      setAssignments([]);
      return;
    }
    const res = await axiosInstance.get(`/admin/roles/${roleId}/permission-groups`);
    setAssignments(res.data || []);
  };

  const handleRoleChange = (e) => {
    const id = e.target.value;
    setSelectedRoleId(id);
    fetchAssignments(id);
  };

  const handleAssign = async () => {
    if (!selectedRoleId || !selectedGroupId) return;
    await axiosInstance.post(`/admin/roles/${selectedRoleId}/permission-groups`, {
      permissionGroupId: selectedGroupId,
    });
    fetchAssignments(selectedRoleId);
  };

  const handleRemove = async (groupId) => {
    if (!selectedRoleId) return;
    await axiosInstance.delete(`/admin/roles/${selectedRoleId}/permission-groups/${groupId}`);
    fetchAssignments(selectedRoleId);
  };

  return (
    <div>
      <h2 className="section-title">Role Permission Groups</h2>
      <div style={{ display: "flex", gap: "1rem", marginBottom: "1rem" }}>
        <select className="form-input" style={{ width: "auto" }} value={selectedRoleId} onChange={handleRoleChange}>
          <option value="">Select Role</option>
          {roles.map(r => <option key={r.id} value={r.id}>{r.name}</option>)}
        </select>
        <select className="form-input" style={{ width: "auto" }} value={selectedGroupId} onChange={e => setSelectedGroupId(e.target.value)}>
          <option value="">Select Group</option>
          {groups.map(g => <option key={g.id} value={g.id}>{g.name}</option>)}
        </select>
        <button className="btn-primary" onClick={handleAssign} style={{ width: "auto" }}>Assign</button>
      </div>

      {selectedRoleId && (
        <div>
          <h3>Assigned Groups</h3>
          {assignments.length === 0 ? <p style={{ color: "#666" }}>No groups assigned.</p> : (
            <ul>
              {assignments.map(groupId => (
                <li key={groupId}>
                  {groups.find(g => g.id === groupId)?.name || groupId}
                  <button className="btn-delete" style={{ marginLeft: "1rem" }} onClick={() => handleRemove(groupId)}>Remove</button>
                </li>
              ))}
            </ul>
          )}
        </div>
      )}
    </div>
  );
};

export default RolePermissionGroupsPage;