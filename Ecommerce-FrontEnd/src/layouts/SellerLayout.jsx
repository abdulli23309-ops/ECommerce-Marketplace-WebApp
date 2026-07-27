import { Outlet, Link, useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { logout } from "../store/authSlice";
import { useState, useEffect } from "react";
import axiosInstance from "../services/axiosInstance";
import { getImageUrl } from "../utils/imageHelper";

const SellerLayout = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [storeId, setStoreId] = useState(null);
  const [storeLogo, setStoreLogo] = useState(null);

  useEffect(() => {
    const fetchStore = async () => {
      try {
        const res = await axiosInstance.get("/seller/store");
        if (res.data) {
          setStoreId(res.data.id);
          setStoreLogo(res.data.logoUrl || null);
        }
      } catch (err) {
        console.error("Failed to load store", err);
      }
    };
    fetchStore();
  }, []);

  const handleLogout = () => {
    dispatch(logout());
    navigate("/login");
  };

  return (
    <div className="dashboard-layout">
      {/* Sidebar */}
      <aside className="dashboard-sidebar">
        <div className="dashboard-header" style={{ display: "flex", alignItems: "center", gap: "0.75rem" }}>
          {storeLogo ? (
            <img
              src={getImageUrl(storeLogo)}
              alt="Store logo"
              style={{ width: "36px", height: "36px", borderRadius: "0.25rem", objectFit: "cover" }}
            />
          ) : (
            <div style={{ width: "36px", height: "36px", background: "#eaeaea", borderRadius: "0.25rem" }} />
          )}
          <h1>Seller Panel</h1>
        </div>
        
        <nav className="dashboard-nav">
          <Link to="/seller/dashboard" className="dashboard-nav-link">
            <svg className="dashboard-nav-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M3 12l2-2m0 0l7-7 7 7m-9 2v10m4-10v10" />
            </svg>
            Dashboard
          </Link>
          <Link to="/seller/products" className="dashboard-nav-link">
            <svg className="dashboard-nav-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" />
            </svg>
            Products
          </Link>
          <Link to="/seller/orders" className="dashboard-nav-link">
            <svg className="dashboard-nav-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01" />
            </svg>
            Orders
          </Link>
          <Link to="/seller/shipments" className="dashboard-nav-link">
  <svg className="dashboard-nav-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M9 17a2 2 0 11-4 0 2 2 0 014 0zM19 17a2 2 0 11-4 0 2 2 0 014 0z" />
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M13 16V6a1 1 0 00-1-1H4a1 1 0 00-1 1v10a1 1 0 001 1h8a1 1 0 001-1zM13 16h6a1 1 0 001-1v-4a1 1 0 00-1-1h-5" />
  </svg>
  Shipments
</Link>
<Link to="/seller/reviews" className="dashboard-nav-link">
  <svg className="dashboard-nav-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M11.049 2.927c.3-.921 1.603-.921 1.902 0l1.519 4.674a1 1 0 00.95.69h4.915c.969 0 1.371 1.24.588 1.81l-3.976 2.888a1 1 0 00-.363 1.118l1.518 4.674c.3.922-.755 1.688-1.538 1.118l-3.976-2.888a1 1 0 00-1.176 0l-3.976 2.888c-.783.57-1.838-.197-1.538-1.118l1.518-4.674a1 1 0 00-.363-1.118l-3.976-2.888c-.784-.57-.38-1.81.588-1.81h4.914a1 1 0 00.951-.69l1.519-4.674z" />
  </svg>
  Reviews
</Link>
          <Link to="/seller/settings" className="dashboard-nav-link">
            <svg className="dashboard-nav-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.066 2.573c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.573 1.066c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.066-2.573c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z" />
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
            </svg>
            Settings
          </Link>
          {storeId && (
            <a
              href={`/store/${storeId}`}
              target="_blank"
              rel="noopener noreferrer"
              className="dashboard-nav-link"
              style={{ marginTop: "auto" }}
            >
              <svg className="dashboard-nav-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M10 6H6a2 2 0 00-2 2v10a2 2 0 002 2h10a2 2 0 002-2v-4M14 4h6m0 0v6m0-6L10 14" />
              </svg>
              View Store
            </a>
          )}
        </nav>
        
        <div className="dashboard-footer">
          <button onClick={handleLogout} className="btn-logout">
            <svg className="dashboard-nav-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
            </svg>
            Sign Out
          </button>
        </div>
      </aside>

      {/* Main Content */}
      <main className="dashboard-main">
        <Outlet />
      </main>
    </div>
  );
};

export default SellerLayout;