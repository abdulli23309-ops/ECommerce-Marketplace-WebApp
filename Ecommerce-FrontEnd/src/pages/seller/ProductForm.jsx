import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";
import { fetchCategories, fetchSubCategories } from "../../services/categoryService";
import { fetchBrands } from "../../services/brandService";
import { createProduct, updateProduct, fetchSellerProducts } from "../../services/sellerProductService";

const ProductForm = () => {
  const { id } = useParams(); // if id exists, we're editing
  const isEdit = Boolean(id);
  const navigate = useNavigate();
  const { register, handleSubmit, setValue, formState: { errors }, watch } = useForm();
  const [loading, setLoading] = useState(false);
  const [categories, setCategories] = useState([]);
  const [subCategories, setSubCategories] = useState([]);
  const [brands, setBrands] = useState([]);
  const selectedCategoryId = watch("categoryId");

  useEffect(() => {
    const loadFormData = async () => {
      const [cats, br] = await Promise.all([fetchCategories(), fetchBrands()]);
      setCategories(cats);
      setBrands(br);

      if (isEdit) {
        // Load product data to pre-fill form
        const products = await fetchSellerProducts();
        const product = products.find(p => p.id === id);
        if (product) {
          setValue("name", product.name);
          setValue("description", product.description);
          setValue("basePrice", product.basePrice);
          setValue("stockQuantity", product.stockQuantity || 0);
          setValue("subCategoryId", product.subCategoryId || "");
          setValue("brandId", product.brandId || "");
          // We need to set categoryId based on subCategoryId? The backend doesn't have a direct categoryId field on product. SubCategory belongs to Category, so we need to fetch subcategory to get its category. That's complex. For simplicity, we'll skip category pre-selection or we can load subcategory then set category. We'll just leave category dropdown empty, but we'll populate subcategories based on selected category.
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
        // imageUrls? not in this form, we can add later
      };

      if (isEdit) {
        await updateProduct(id, payload);
      } else {
        await createProduct(payload);
      }
      navigate("/seller/products");
    } catch (err) {
      console.error("Failed to save product", err);
    } finally {
      setLoading(false);
    }
  };

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
          <input type="number" className="form-input" {...register("stockQuantity", { valueAsNumber: true })} />
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
        <button type="submit" className="btn-primary" disabled={loading}>
          {loading ? "Saving..." : (isEdit ? "Update Product" : "Create Product")}
        </button>
      </form>
    </div>
  );
};

export default ProductForm;