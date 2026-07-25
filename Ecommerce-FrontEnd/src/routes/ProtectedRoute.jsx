import { Navigate, Outlet } from "react-router-dom";
import { useSelector } from "react-redux";

const ProtectedRoute = ({ allowedRoles = [] }) => {
  const { user, accessToken } = useSelector((state) => state.auth);

  if (!accessToken) return <Navigate to="/login" replace />;

  // User has at least one of the allowed roles
  const hasRole =
    allowedRoles.length === 0 ||
    user?.roles?.some((role) => allowedRoles.includes(role));

  if (!hasRole) return <Navigate to="/" replace />;

  return <Outlet />;
};

export default ProtectedRoute;