import { useForm } from "react-hook-form";
import { useDispatch } from "react-redux";
import { useNavigate, Link } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import { loginUser } from "../../services/authService";
import { setCredentials } from "../../store/authSlice";

const LoginPage = () => {
  const { register, handleSubmit, formState: { errors }, setError } = useForm();
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const onSubmit = async (data) => {
    try {
      const result = await loginUser(data.email, data.password);
      if (!result.succeeded) {
        setError("root", { message: result.message || "Login failed" });
        return;
      }

      const decoded = jwtDecode(result.accessToken);
      let roles = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      if (!roles) roles = [];
      if (!Array.isArray(roles)) roles = [roles];

      const user = {
        id: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
        email: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
        fullName: decoded["fullName"],
        roles,
      };

      dispatch(setCredentials({ user, accessToken: result.accessToken, refreshToken: result.refreshToken }));

      if (roles.includes("SuperAdmin")) {
        navigate("/admin/dashboard");
      } else if (roles.includes("Seller")) {
        navigate("/seller/dashboard");
      } else {
        navigate("/");
      }
    } catch (err) {
      setError("root", { message: "Network error. Please try again." });
    }
  };

  return (
    <div>
      <h1 className="auth-title">Log In</h1>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="form-group">
          <label className="form-label">Email</label>
          <input
            type="email"
            {...register("email", { required: true })}
            className="form-input"
            placeholder="name@example.com"
          />
          {errors.email && <p className="error-text">Email is required</p>}
        </div>
        
        <div className="form-group">
          <label className="form-label">Password</label>
          <input
            type="password"
            {...register("password", { required: true })}
            className="form-input"
            placeholder="••••••••"
          />
          {errors.password && <p className="error-text">Password is required</p>}
        </div>
        
        {errors.root && <p className="error-text">{errors.root.message}</p>}
        
        <button type="submit" className="btn-primary">
          Sign In
        </button>
      </form>
      
      <p className="auth-footer">
        Don't have an account?{" "}
        <Link to="/register" className="auth-link">
          Create one
        </Link>
      </p>
    </div>
  );
};

export default LoginPage;