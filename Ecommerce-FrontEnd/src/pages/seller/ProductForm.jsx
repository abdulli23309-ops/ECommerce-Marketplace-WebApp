import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";
import { fetchCategories, fetchSubCategories } from "../../services/categoryService";
import { fetchBrands } from "../../services/brandService";
import {
  createProduct,
  updateProduct,
  fetchSellerProducts,
  uploadProductImage,
  deleteProductImage,
} from "../../services/sellerProductService";
import axiosInstance from "../../services/axiosInstance";

const ProductForm = () => {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();
  const { register, handleSubmit, setValue, formState: { errors }, watch } = useForm();
  const [loading, setLoading] = useState(false);
  const [categories, setCategories] = useState([]);
  const [subCategories, setSubCategories] = useState([]);
  const [brands, setBrands] = useState([]);
  const selectedCategoryId = watch("categoryId");

  // Image states
  const [existingImages, setExistingImages] = useState([]);
  const [newFiles, setNewFiles] = useState([]);   // File objects selected for upload
  const [uploading, setUploading] = useState(false);
  const [imageError, setImageError] = useState(null);

  useEffect(() => {
    const loadFormData = async () => {
      const [cats, br] = await Promise.all([fetchCategories(), fetchBrands()]);
      setCategories(cats);
      setBrands(br);

      if (isEdit) {
        const products = await fetchSellerProducts();
        const product = products.find(p => p.id === id);
        if (product) {
          setValue("name", product.name);
          setValue("description", product.description);
          setValue("basePrice", product.basePrice);
          setValue("stockQuantity", product.stockQuantity || 0);
          // Set category/subcategory if available
          // (we may need to load subcategory->category mapping, but we'll skip for simplicity; the form currently expects categoryId separate)
          setValue("brandId", product.brandId || "");

          // Load existing images
          setExistingImages(product.images || []);
        }
      }
    };
    loadFormData();
  }, [id, isEdit, setValue]);

  // Fetch subcategories when category changes
  useEffect(() => {
    if (selectedCategoryId) {
      fetchSubCategories(selectedCategoryId).then(setSubCategories);
    } else {
      setSubCategories([]);
    }
  }, [selectedCategoryId]);

  const onSubmit = async (data) => {
    setLoading(true);
    try {
      const payload = {
        name: data.name,
        description: data.description,
        basePrice: parseFloat(data.basePrice),
        stockQuantity: parseInt(data.stockQuantity) || 0,
        subCategoryId: data.subCategoryId || null,
        brandId: data.brandId || null,
      };

      let productId = id;
      if (isEdit) {
        await updateProduct(productId, payload);
      } else {
        const newProduct = await createProduct(payload);
        productId = newProduct.id;
      }

      // Upload any selected new files
      if (newFiles.length > 0 && productId) {
        setUploading(true);
        for (const file of newFiles) {
          await uploadProductImage(productId, file);
        }
        setUploading(false);
      }

      navigate("/seller/products");
    } catch (err) {
      console.error("Failed to save product", err);
      setLoading(false);
      setUploading(false);
    }
  };

  const handleDeleteImage = async (imageId) => {
    if (!window.confirm("Delete this image?")) return;
    try {
      await deleteProductImage(id, imageId);
      setExistingImages(prev => prev.filter(img => img.id !== imageId));
    } catch (err) {
      console.error("Failed to delete image", err);
    }
  };

  const handleFileChange = (e) => {
    const files = Array.from(e.target.files);
    // Validate file size/type? Backend does it, but we can show error earlier.
    setNewFiles(files);
    setImageError(null);
  };

  const apiBaseUrl = import.meta.env.VITE_API_BASE_URL?.replace(/\/api$/, ''); // remove trailing /api to get root URL

  return (
    <div>
      <h2 className="section-title">{isEdit ? "Edit Product" : "Add Product"}</h2>
      <form onSubmit={handleSubmit(onSubmit)} className="form-container" style={{ maxWidth: "600px" }}>
        <div className="form-group">
          <label className="form-label">Name</label>
          <input className="form-input" {...register("name", { required: true })} />
          {errors.name && <p className="error-text">Name is required</p>}
        </div>
        <div className="form-group">
          <label className="form-label">Description</label>
          <textarea className="form-input" rows="4" {...register("description")} />
        </div>
        <div className="form-group">
          <label className="form-label">Price (PKR)</label>
          <input type="number" step="0.01" className="form-input" {...register("basePrice", { required: true, min: 0.01 })} />
          {errors.basePrice && <p className="error-text">Valid price is required</p>}
        </div>
        <div className="form-group">
  <label className="form-label">Stock Quantity</label>
  <input
    type="number"
    className="form-input"
    {...register("stockQuantity", { required: true, valueAsNumber: true, min: 0 })}
  />
  {errors.stockQuantity && <p className="error-text">Stock quantity is required</p>}
</div>
        <div className="form-group">
          <label className="form-label">Category</label>
          <select className="form-input" {...register("categoryId")} defaultValue="">
            <option value="">Select Category</option>
            {categories.map(cat => (
              <option key={cat.id} value={cat.id}>{cat.name}</option>
            ))}
          </select>
        </div>
        <div className="form-group">
          <label className="form-label">SubCategory</label>
          <select className="form-input" {...register("subCategoryId")} defaultValue="">
            <option value="">Select SubCategory</option>
            {subCategories.map(sub => (
              <option key={sub.id} value={sub.id}>{sub.name}</option>
            ))}
          </select>
        </div>
        <div className="form-group">
          <label className="form-label">Brand</label>
          <select className="form-input" {...register("brandId")} defaultValue="">
            <option value="">Select Brand</option>
            {brands.map(brand => (
              <option key={brand.id} value={brand.id}>{brand.name}</option>
            ))}
          </select>
        </div>

        {/* Existing Images (edit mode) */}
        {isEdit && existingImages.length > 0 && (
          <div className="form-group">
            <label className="form-label">Current Images</label>
            <div className="image-gallery">
              {existingImages.map(img => (
                <div key={img.id} className="image-thumb">
                  <img src={`${apiBaseUrl}${img.imageUrl}`} alt="Product" />
                  <button type="button" className="btn-remove" onClick={() => handleDeleteImage(img.id)}>Remove</button>
                </div>
              ))}
            </div>
          </div>
        )}

        {/* New Image Upload */}
        <div className="form-group">
          <label className="form-label">{isEdit ? "Add More Images" : "Product Images"}</label>
          <input type="file" multiple accept=".jpg,.jpeg,.png,.webp" onChange={handleFileChange} />
          {newFiles.length > 0 && (
            <div className="new-files-preview">
              {newFiles.map((file, index) => (
                <div key={index} className="file-preview">
                  <img src={URL.createObjectURL(file)} alt="Preview" />
                  <span>{file.name}</span>
                </div>
              ))}
            </div>
          )}
          {imageError && <p className="error-text">{imageError}</p>}
        </div>

        <button type="submit" className="btn-primary" disabled={loading || uploading}>
          {loading || uploading ? (uploading ? "Uploading images..." : "Saving...") : (isEdit ? "Update Product" : "Create Product")}
        </button>
      </form>
    </div>
  );
};

export default ProductForm;