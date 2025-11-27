import { Navigate } from "react-router-dom";

export default function ProtectedRoute({ allowedRoles, children }) {
    const token = localStorage.getItem("token");
    const role = localStorage.getItem("role");

    // Nếu chưa login → bắt về login
    if (!token) return <Navigate to="/login" replace />;

    // Nếu không truyền allowedRoles → cho qua luôn
    if (!allowedRoles || allowedRoles.length === 0) {
        return children;
    }

    // Nếu có allowedRoles → kiểm tra quyền
    if (!allowedRoles.includes(role)) {
        return <Navigate to="/login" replace />;
    }

    return children;
}
