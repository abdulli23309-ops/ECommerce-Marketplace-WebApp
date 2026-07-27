import { Routes, Route } from "react-router-dom";
import AuthLayout from "./layouts/AuthLayout";
import CustomerLayout from "./layouts/CustomerLayout";
import SellerLayout from "./layouts/SellerLayout";
import AdminLayout from "./layouts/AdminLayout";
import ProtectedRoute from "./routes/ProtectedRoute";
import NotFound from "./pages/NotFound";
import LoginPage from "./pages/auth/LoginPage";
import RegisterPage from "./pages/auth/RegisterPage";
import HomePage from "./pages/customer/HomePage";
import CartPage from "./pages/customer/CartPage";
import OrderHistoryPage from "./pages/customer/OrderHistoryPage";
import ProfilePage from "./pages/customer/ProfilePage";
import CheckoutPage from "./pages/customer/CheckoutPage";
import SellerProductsPage from "./pages/seller/SellerProductsPage";
import ProductForm from "./pages/seller/ProductForm";
import SellerOrdersPage from "./pages/seller/SellerOrdersPage";
import SellerApprovalPage from "./pages/admin/SellerApprovalPage";
import ProductModerationPage from "./pages/admin/ProductModerationPage";
import ReturnsManagementPage from "./pages/admin/ReturnsManagementPage";
import RefundManagementPage from "./pages/admin/RefundManagementPage";
import ProductDetailPage from "./pages/customer/ProductDetailPage";
import SellerPendingPage from "./pages/seller/SellerPendingPage";
import SellerRegisterPage from "./pages/seller/SellerRegisterPage";
import AdminDashboardPage from "./pages/admin/AdminDashboardPage";
import AddressBookPage from "./pages/customer/AddressBookPage";
import ProductListingPage from "./pages/customer/ProductListingPage";
import StoreSettingsPage from "./pages/seller/StoreSettingsPage";
import StorePage from "./pages/customer/StorePage";
import OrderDetailPage from "./pages/customer/OrderDetailPage";
import SellerReviewsPage from "./pages/seller/SellerReviewsPage";
import SellerDashboardPage from "./pages/seller/SellerDashboardPage";
import ShipmentManagementPage from "./pages/seller/ShipmentManagementPage";
import ReviewPage from "./pages/customer/ReviewPage";
import MyReviewsPage from "./pages/customer/MyReviewsPage";
import ReviewDetailPage from "./pages/customer/ReviewDetailPage";


const App = () => {
  return (
    <Routes>
      {/* Public auth pages – NOT protected */}
      <Route element={<AuthLayout />}>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
      </Route>

      {/*
        PUBLIC storefront routes — NOT behind ProtectedRoute.
        This was the single biggest bug found in the QA review: the home page
        and product browsing were previously locked behind ProtectedRoute
        allowedRoles={["Customer"]}, meaning a first-time visitor with no
        account was redirected straight to /login and could never see the
        site. CustomerLayout already renders correctly for a logged-out user
        (it shows "Sign In" instead of "Profile" — see its user ? ... : ...
        branch), so it's safe to render it outside auth entirely.

        NOTE: "/products" still points at the placeholder <div>Products</div>
        and there is still no "/products/:id" route or backing
        GET /api/products/{id} endpoint. Building the real listing + detail
        pages is Phase 1 work, not part of this Phase 0 security/routing fix —
        flagging so this isn't mistaken for "already done."
      */}
      <Route element={<CustomerLayout />}>
        <Route path="/" element={<HomePage />} />
       <Route path="/products" element={<ProductListingPage />} />
        <Route path="/products/:productId" element={<ProductDetailPage />} />
        <Route path="/store/:storeId" element={<StorePage />} />
      </Route>

      {/* Customer routes that legitimately require an account – protected, allowed roles: Customer */}
      <Route element={<ProtectedRoute allowedRoles={["Customer"]} />}>
        <Route element={<CustomerLayout />}>
          <Route path="/cart" element={<CartPage />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/checkout" element={<CheckoutPage />} />
          <Route path="/orders" element={<OrderHistoryPage />} />
          <Route path="/addresses" element={<AddressBookPage />} />
          <Route path="/orders/:orderId" element={<OrderDetailPage />} />
          <Route path="/review/new/:orderItemId" element={<ReviewPage />} />
          <Route path="/reviews/my" element={<MyReviewsPage />} />
          <Route path="/reviews/:reviewId" element={<ReviewDetailPage />} />
        </Route>
      </Route>
      {/* Authenticated-only routes (any role) */}
<Route element={<ProtectedRoute />}>   {/* no allowedRoles = any logged‑in user */}
  <Route element={<CustomerLayout />}>
    <Route path="/seller/register" element={<SellerRegisterPage />} />
    <Route path="/seller/pending" element={<SellerPendingPage />} />
  </Route>
</Route>

     {/* Inside Seller routes */}
<Route element={<ProtectedRoute allowedRoles={["Seller"]} />}>
  <Route element={<SellerLayout />}>
    <Route path="/seller/dashboard" element={<SellerDashboardPage />} />
    <Route path="/seller/products" element={<SellerProductsPage />} />
    <Route path="/seller/products/new" element={<ProductForm />} />
    <Route path="/seller/products/edit/:id" element={<ProductForm />} />
    <Route path="/seller/orders" element={<SellerOrdersPage />} />
    <Route path="/seller/register" element={<SellerRegisterPage />} />
    <Route path="/seller/settings" element={<StoreSettingsPage />} />
    <Route path="/seller/shipments" element={<ShipmentManagementPage />} />
    <Route path="/seller/reviews" element={<SellerReviewsPage />} />
<Route path="/seller/pending" element={<SellerPendingPage />} />
  </Route>
</Route>

      {/* Admin routes – protected, allowed roles: SuperAdmin */}
      <Route element={<ProtectedRoute allowedRoles={["SuperAdmin"]} />}>
        <Route element={<AdminLayout />}>
          <Route path="/admin/sellers" element={<SellerApprovalPage />} />
          <Route path="/admin/products" element={<ProductModerationPage />} />
          <Route path="/admin/returns" element={<ReturnsManagementPage />} />
          <Route path="/admin/refunds" element={<RefundManagementPage />} />
          <Route path="/admin/dashboard" element={<AdminDashboardPage />} />
        </Route>
      </Route>

      {/* 404 */}
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
};

export default App;