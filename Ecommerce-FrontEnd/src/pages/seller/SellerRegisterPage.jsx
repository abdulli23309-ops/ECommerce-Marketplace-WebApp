import { useState } from "react";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../../services/axiosInstance";
const SellerRegisterPage = () => {
  const navigate = useNavigate();
  const [step, setStep] = useState(1);
  const [profile, setProfile] = useState({ businessName: "", description: "" });
  const [store, setStore] = useState({ name: "", description: "" });
  const [error, setError] = useState(null);

  const handleProfileSubmit = async (e) => {
    e.preventDefault();
    try {
      await axiosInstance.post("/seller/profile", profile);
      setStep(2);
    } catch (err) {
      setError("Failed to create profile. Please try again.");
    }
  };

  const handleStoreSubmit = async (e) => {
    e.preventDefault();
    try {
      await axiosInstance.post("/seller/store", store);
      navigate("/seller/pending");
    } catch (err) {
      setError("Failed to create store. Please try again.");
    }
  };

  return (
    <div style={{ maxWidth: "600px", margin: "2rem auto", padding: "2rem" }}>
      <h2 className="section-title">Become a Seller</h2>
      {step === 1 && (
        <form onSubmit={handleProfileSubmit}>
          <h3>Step 1: Business Profile</h3>
          <div className="form-group">
            <label className="form-label">Business Name</label>
            <input
              className="form-input"
              value={profile.businessName}
              onChange={(e) => setProfile({ ...profile, businessName: e.target.value })}
              required
            />
          </div>
          <div className="form-group">
            <label className="form-label">Description (optional)</label>
            <textarea
              className="form-input"
              value={profile.description}
              onChange={(e) => setProfile({ ...profile, description: e.target.value })}
            />
          </div>
          {error && <p className="error-text">{error}</p>}
          <button type="submit" className="btn-primary">Next</button>
        </form>
      )}
      {step === 2 && (
        <form onSubmit={handleStoreSubmit}>
          <h3>Step 2: Your Store</h3>
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
            <label className="form-label">Store Description (optional)</label>
            <textarea
              className="form-input"
              value={store.description}
              onChange={(e) => setStore({ ...store, description: e.target.value })}
            />
          </div>
          {error && <p className="error-text">{error}</p>}
          <button type="submit" className="btn-primary">Submit</button>
        </form>
      )}
    </div>
  );
};

export default SellerRegisterPage;