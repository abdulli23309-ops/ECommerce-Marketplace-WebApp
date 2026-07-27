import { useState, useEffect } from "react";
import axiosInstance from "../../services/axiosInstance";
import { useSelector, useDispatch } from "react-redux";
import { setCredentials } from "../../store/authSlice";

const SellerSettingsPage = () => {
  const dispatch = useDispatch();
  const { user } = useSelector((state) => state.auth);

  // ---------- Profile tab state ----------
  const [fullName, setFullName] = useState(user?.fullName || "");
  const [email, setEmail] = useState(user?.email || "");
  const [profileLoading, setProfileLoading] = useState(false);
  const [profileMsg, setProfileMsg] = useState({ text: "", type: "" });

  // ---------- Store tab state ----------
  const [store, setStore] = useState({ name: "", description: "", logoUrl: "" });
  const [storeLoading, setStoreLoading] = useState(false);
  const [storeMsg, setStoreMsg] = useState({ text: "", type: "" });
  const [logoPreview, setLogoPreview] = useState(null);   // always shows current or new logo
  const [uploadingLogo, setUploadingLogo] = useState(false);

  const [activeTab, setActiveTab] = useState("profile");

  // Load store data on mount – set the logo preview from stored logoUrl
  useEffect(() => {
    const fetchStore = async () => {
      try {
        const res = await axiosInstance.get("/seller/store");
        if (res.data) {
          setStore({
            name: res.data.name || "",
            description: res.data.description || "",
            logoUrl: res.data.logoUrl || "",
          });
          // Set preview from stored logo (or null)
          if (res.data.logoUrl) {
            const base = import.meta.env.VITE_API_BASE_URL?.replace(/\/api$/, "") || "";
            setLogoPreview(res.data.logoUrl.startsWith("http") ? res.data.logoUrl : `${base}${res.data.logoUrl}`);
          } else {
            setLogoPreview(null);   // no logo yet
          }
        }
      } catch (err) {
        console.error("Failed to load store", err);
      }
    };
    fetchStore();
  }, []);

  // ---------- Profile submit ----------
  const handleProfileSubmit = async (e) => {
    e.preventDefault();
    setProfileLoading(true);
    setProfileMsg({ text: "", type: "" });
    try {
      const res = await axiosInstance.put("/Account/profile", { fullName, email });
      if (res.data.succeeded) {
        dispatch(setCredentials({
          user: { ...user, fullName, email },
          accessToken: user.accessToken,
          refreshToken: user.refreshToken,
        }));
        setProfileMsg({ text: "Profile updated.", type: "success" });
      } else {
        setProfileMsg({ text: res.data.message || "Update failed.", type: "error" });
      }
    } catch (err) {
      setProfileMsg({ text: "Network error.", type: "error" });
    } finally {
      setProfileLoading(false);
    }
  };

  // ---------- Store submit ----------
  const handleStoreSubmit = async (e) => {
    e.preventDefault();
    setStoreLoading(true);
    setStoreMsg({ text: "", type: "" });
    try {
      await axiosInstance.put("/seller/store", {
        name: store.name,
        description: store.description,
        logoUrl: store.logoUrl,
      });
      setStoreMsg({ text: "Store updated.", type: "success" });
    } catch (err) {
      setStoreMsg({ text: "Failed to save store.", type: "error" });
    } finally {
      setStoreLoading(false);
    }
  };

  // ---------- Logo upload (replace preview immediately) ----------
  const handleLogoChange = async (e) => {
    const file = e.target.files[0];
    if (!file) return;

    // Show local preview instantly
    setLogoPreview(URL.createObjectURL(file));
    setUploadingLogo(true);
    setStoreMsg({ text: "", type: "" });

    try {
      const formData = new FormData();
      formData.append("file", file);
      const res = await axiosInstance.post("/seller/store/logo", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });
      // Replace preview with the actual uploaded URL
      const newUrl = res.data.logoUrl;
      const base = import.meta.env.VITE_API_BASE_URL?.replace(/\/api$/, "") || "";
      setLogoPreview(newUrl.startsWith("http") ? newUrl : `${base}${newUrl}`);
      setStore({ ...store, logoUrl: newUrl });
    } catch (err) {
      setStoreMsg({ text: "Logo upload failed.", type: "error" });
      // Revert to previous logo (if any)
      if (store.logoUrl) {
        const base = import.meta.env.VITE_API_BASE_URL?.replace(/\/api$/, "") || "";
        setLogoPreview(store.logoUrl.startsWith("http") ? store.logoUrl : `${base}${store.logoUrl}`);
      } else {
        setLogoPreview(null);
      }
    } finally {
      setUploadingLogo(false);
    }
  };

  // Helper for absolute image URLs
  const getImageUrl = (url) => {
    if (!url) return "";
    if (url.startsWith("http")) return url;
    const base = import.meta.env.VITE_API_BASE_URL?.replace(/\/api$/, "") || "";
    return `${base}${url}`;
  };

  return (
    <div style={{ maxWidth: "700px", margin: "0 auto", padding: "2rem" }}>
      <h2 className="section-title">Settings</h2>

      {/* Tabs */}
      <div style={{ display: "flex", gap: "2rem", marginBottom: "2rem", borderBottom: "1px solid #eaeaea" }}>
        <button
          onClick={() => setActiveTab("profile")}
          style={{
            padding: "0.5rem 0",
            border: "none",
            background: "none",
            fontWeight: activeTab === "profile" ? 600 : 400,
            color: activeTab === "profile" ? "#000" : "#666",
            borderBottom: activeTab === "profile" ? "2px solid #000" : "2px solid transparent",
            cursor: "pointer",
          }}
        >
          Profile
        </button>
        <button
          onClick={() => setActiveTab("store")}
          style={{
            padding: "0.5rem 0",
            border: "none",
            background: "none",
            fontWeight: activeTab === "store" ? 600 : 400,
            color: activeTab === "store" ? "#000" : "#666",
            borderBottom: activeTab === "store" ? "2px solid #000" : "2px solid transparent",
            cursor: "pointer",
          }}
        >
          Store
        </button>
      </div>

      {/* PROFILE TAB */}
      {activeTab === "profile" && (
        <form onSubmit={handleProfileSubmit}>
          <div className="form-group">
            <label className="form-label">Full Name</label>
            <input
              className="form-input"
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
              required
            />
          </div>
          <div className="form-group">
            <label className="form-label">Email</label>
            <input
              className="form-input"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>
          {profileMsg.text && (
            <p style={{ color: profileMsg.type === "success" ? "#000" : "#d11a2a", marginBottom: "1rem", fontWeight: 500 }}>
              {profileMsg.text}
            </p>
          )}
          <button type="submit" className="btn-primary" disabled={profileLoading}>
            {profileLoading ? "Saving..." : "Save Profile"}
          </button>
        </form>
      )}

      {/* STORE TAB */}
      {activeTab === "store" && (
        <form onSubmit={handleStoreSubmit}>
          <div className="form-group">
            <label className="form-label">Store Name</label>
            <input
              className="form-input"
              value={store.name}
              onChange={(e) => setStore({ ...store, name: e.target.value })}
              required
            />
          </div>
          <div className="form-group">
            <label className="form-label">Store Description</label>
            <textarea
              className="form-input"
              rows={4}
              value={store.description}
              onChange={(e) => setStore({ ...store, description: e.target.value })}
            />
          </div>

          {/* Logo – always show current logo or placeholder */}
          <div className="form-group">
            <label className="form-label">Store Logo</label>
            {logoPreview ? (
              <div style={{ marginBottom: "0.5rem" }}>
                <img
                  src={logoPreview}
                  alt="Store logo"
                  style={{ width: "120px", height: "120px", objectFit: "cover", border: "1px solid #eaeaea", borderRadius: "0.25rem" }}
                />
              </div>
            ) : (
              <p style={{ color: "#666", marginBottom: "0.5rem", fontStyle: "italic" }}>No logo uploaded yet.</p>
            )}
            <input
              type="file"
              accept=".jpg,.jpeg,.png,.webp"
              onChange={handleLogoChange}
              style={{ marginBottom: "0.5rem" }}
            />
            {uploadingLogo && <p style={{ color: "#666" }}>Uploading logo...</p>}
          </div>

          {storeMsg.text && (
            <p style={{ color: storeMsg.type === "success" ? "#000" : "#d11a2a", marginBottom: "1rem", fontWeight: 500 }}>
              {storeMsg.text}
            </p>
          )}
          <button type="submit" className="btn-primary" disabled={storeLoading || uploadingLogo}>
            {storeLoading ? "Saving..." : "Save Store"}
          </button>
        </form>
      )}
    </div>
  );
};

export default SellerSettingsPage;