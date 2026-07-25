import { useState, useEffect } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import { useSelector } from "react-redux";
import { fetchProductById, fetchApprovedProducts } from "../../services/productService";
import { fetchProductReviews } from "../../services/reviewService";
import { addToCart } from "../../services/cartService";

const ProductDetailPage = () => {
  const { productId } = useParams();
  const navigate = useNavigate();
  const { user } = useSelector((state) => state.auth);

  // Data States
  const [product, setProduct] = useState(null);
  const [reviews, setReviews] = useState([]);
  const [relatedProducts, setRelatedProducts] = useState([]);
  const [mainImage, setMainImage] = useState("");

  // UI States
  const [loading, setLoading] = useState(true);
  const [addingToCart, setAddingToCart] = useState(false);
  const [quantity, setQuantity] = useState(1);
  const [message, setMessage] = useState({ text: "", type: "" });

  useEffect(() => {
    const loadProductData = async () => {
      setLoading(true);
      try {
        // 1. Fetch main product details
        const productData = await fetchProductById(productId);
        setProduct(productData);
        
        if (productData?.images?.length > 0) {
          setMainImage(productData.images[0].imageUrl);
        }

        // 2. Fetch reviews (fail gracefully)
        try {
          const reviewData = await fetchProductReviews(productId);
          setReviews(reviewData || []);
        } catch (error) {
          console.error("Failed to fetch reviews:", error);
          setReviews([]);
        }

        // 3. Fetch related products (client-side filter as temporary fix)
        try {
          const allProducts = await fetchApprovedProducts(1, 100);
          const related = (allProducts?.items || [])
            .filter((p) => p.storeId === productData.storeId && p.id !== productId)
            .slice(0, 4);
          setRelatedProducts(related);
        } catch (error) {
          console.error("Failed to fetch related products:", error);
          setRelatedProducts([]);
        }

      } catch (error) {
        console.error("Failed to load product details:", error);
        setProduct(null);
      } finally {
        setLoading(false);
        window.scrollTo(0, 0); // Reset scroll to top on new product load
      }
    };

    loadProductData();
  }, [productId]);

  const handleAddToCart = async () => {
    if (!user) {
      navigate("/login");
      return;
    }

    setAddingToCart(true);
    setMessage({ text: "", type: "" });

    try {
      await addToCart(product.id, quantity);
      setMessage({ text: "Item added to cart.", type: "success" });
      setTimeout(() => setMessage({ text: "", type: "" }), 3000);
    } catch (error) {
      console.error("Cart error:", error);
      setMessage({ text: "Could not add to cart. Please try again.", type: "error" });
    } finally {
      setAddingToCart(false);
    }
  };

  if (loading) {
    return (
      <div style={{ padding: "4rem", textAlign: "center", color: "#333" }}>
        <p>Loading product details...</p>
      </div>
    );
  }

  if (!product) {
    return (
      <div style={{ padding: "4rem", textAlign: "center", color: "#333" }}>
        <h2>Product not found</h2>
        <p>This item may have been removed or is currently unavailable.</p>
        <button 
          onClick={() => navigate("/")} 
          style={{ 
            marginTop: "1.5rem", 
            padding: "0.75rem 1.5rem", 
            cursor: "pointer", 
            background: "#000", 
            color: "#fff", 
            border: "none",
            fontWeight: "bold"
          }}
        >
          Return to Homepage
        </button>
      </div>
    );
  }

  return (
    <div className="product-detail-page">
      <div className="product-detail-container">
        
        {/* --- Image Gallery --- */}
        <div className="product-detail-gallery">
          <div className="product-detail-main-image">
            {mainImage ? (
              <img src={mainImage} alt={product.name} />
            ) : (
              <div className="no-image" style={{ background: "#eaeaea", height: "400px", display: "flex", alignItems: "center", justifyContent: "center" }}>
                <span style={{ color: "#666" }}>No Image</span>
              </div>
            )}
          </div>
          
          {product.images?.length > 1 && (
            <div className="product-detail-thumbnails" style={{ display: "flex", gap: "1rem", marginTop: "1rem", overflowX: "auto" }}>
              {product.images.map((img, index) => (
                <img
                  key={index}
                  src={img.imageUrl}
                  alt={`${product.name} thumbnail ${index + 1}`}
                  onClick={() => setMainImage(img.imageUrl)}
                  style={{ 
                    width: "80px", 
                    height: "80px", 
                    objectFit: "cover", 
                    cursor: "pointer",
                    border: mainImage === img.imageUrl ? "2px solid #000" : "1px solid #eaeaea",
                    opacity: mainImage === img.imageUrl ? 1 : 0.6 
                  }}
                />
              ))}
            </div>
          )}
        </div>

        {/* --- Product Info & Actions --- */}
        <div className="product-detail-info">
          <h1 className="product-detail-name" style={{ margin: "0 0 0.5rem 0" }}>{product.name}</h1>
          <p className="product-detail-price" style={{ fontSize: "1.5rem", fontWeight: "bold", margin: "0 0 1.5rem 0" }}>
            PKR {product.basePrice?.toLocaleString()}
          </p>
          
          <div className="product-meta" style={{ color: "#666", fontSize: "0.9rem", marginBottom: "2rem", display: "flex", flexDirection: "column", gap: "0.5rem" }}>
            {product.brandName && <span><strong>Brand:</strong> {product.brandName}</span>}
            {product.categoryName && <span><strong>Category:</strong> {product.categoryName}</span>}
            <span><strong>Store:</strong> {product.storeName}</span>
            <span><strong>Status:</strong> {product.status}</span>
          </div>
          
          <p className="product-detail-description" style={{ lineHeight: "1.6", marginBottom: "2rem" }}>
            {product.description || "No description available for this product."}
          </p>

          {/* Cart Controls */}
          {product.stockQuantity === 0 ? (
            <p className="out-of-stock" style={{ color: "#000", fontWeight: "bold", padding: "1rem", border: "1px solid #000", textAlign: "center" }}>
              Currently Out of Stock
            </p>
          ) : (
            <div className="add-to-cart-row" style={{ display: "flex", gap: "1rem", alignItems: "center" }}>
              <div className="quantity-control" style={{ display: "flex", border: "1px solid #eaeaea" }}>
                <button 
                  onClick={() => setQuantity(Math.max(1, quantity - 1))}
                  style={{ padding: "0.75rem 1rem", background: "none", border: "none", cursor: "pointer", fontSize: "1.2rem" }}
                >−</button>
                <span style={{ padding: "0.75rem 1.5rem", borderLeft: "1px solid #eaeaea", borderRight: "1px solid #eaeaea", display: "flex", alignItems: "center" }}>
                  {quantity}
                </span>
                <button 
                  onClick={() => setQuantity(quantity + 1)}
                  style={{ padding: "0.75rem 1rem", background: "none", border: "none", cursor: "pointer", fontSize: "1.2rem" }}
                >+</button>
              </div>
              <button 
                className="btn-add-to-cart" 
                onClick={handleAddToCart} 
                disabled={addingToCart}
                style={{ 
                  flex: 1, 
                  padding: "0.85rem", 
                  background: "#000", 
                  color: "#fff", 
                  border: "none", 
                  cursor: addingToCart ? "not-allowed" : "pointer",
                  fontWeight: "bold",
                  textTransform: "uppercase",
                  letterSpacing: "1px"
                }}
              >
                {addingToCart ? "Adding..." : "Add to Cart"}
              </button>
            </div>
          )}

          {/* Feedback Message */}
          {message.text && (
            <p style={{ 
              marginTop: "1rem", 
              padding: "0.75rem", 
              border: `1px solid ${message.type === "success" ? "#000" : "#333"}`,
              backgroundColor: message.type === "success" ? "#f9f9f9" : "#fff",
              color: "#000",
              fontWeight: "500",
              textAlign: "center"
            }}>
              {message.text}
            </p>
          )}
        </div>
      </div>

      {/* --- Reviews Section --- */}
      <div className="reviews-section">
        <h2 className="section-title">Customer Reviews</h2>
        {reviews.length === 0 ? (
          <p style={{ color: "#666", fontStyle: "italic" }}>No reviews have been left for this product yet.</p>
        ) : (
          <div className="reviews-list">
            {reviews.map((review) => (
              <div key={review.id} className="review-card">
                <div className="review-header">
                  <span className="review-rating">
                    {'★'.repeat(review.rating)}{'☆'.repeat(5 - review.rating)}
                  </span>
                  <span className="review-date">{new Date(review.createdAt).toLocaleDateString()}</span>
                </div>
                <p className="review-comment">{review.comment}</p>
                <p className="review-author">— {review.userName || "Anonymous"}</p>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* --- Related Products Section --- */}
      {relatedProducts.length > 0 && (
        <div className="related-products-section">
          <h2 className="section-title">More from {product.storeName}</h2>
          <div className="product-grid" style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(200px, 1fr))", gap: "2rem" }}>
            {relatedProducts.map((rp) => (
              <Link to={`/products/${rp.id}`} key={rp.id} className="product-card" style={{ textDecoration: "none", color: "inherit" }}>
                <div className="product-image">
                  {rp.images?.length > 0 ? (
                    <img src={rp.images[0].imageUrl || rp.images[0]} alt={rp.name} />
                  ) : (
                    <div style={{ background: "#eaeaea", height: "100%", display: "flex", alignItems: "center", justifyContent: "center", color: "#666" }}>
                      No Image
                    </div>
                  )}
                </div>
                <div className="product-details" style={{ paddingTop: "1rem" }}>
                  <p className="product-name" style={{ fontWeight: "bold", margin: "0 0 0.25rem 0" }}>{rp.name}</p>
                  <p className="product-price" style={{ color: "#333", margin: 0 }}>PKR {rp.basePrice?.toLocaleString()}</p>
                </div>
              </Link>
            ))}
          </div>
        </div>
      )}
    </div>
  );
};

export default ProductDetailPage;