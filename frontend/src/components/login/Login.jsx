import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { GoogleLogin } from "@react-oauth/google";
import { jwtDecode } from "jwt-decode";
import authApi from "../../api/auth";

export default function LoginPage() {
    const navigate = useNavigate();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [message, setMessage] = useState("");
    const [loading, setLoading] = useState(false);

    // ========================
    // LOGIN (Basic)
    // ========================
    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage("");
        setLoading(true);

        try {
            const res = await authApi.login({ email, password });

            console.log("Login response:", res);
            localStorage.setItem("token", res.token);
            localStorage.setItem("role", res.role);

            if (res.requireChangePassword) {
                navigate("/change-password-first-login");
                return;
            }
            if (res.role === "Admin") {
                navigate("/admin/dashboard");
            }
            else if (res.role === "DepartmentHead") {
                navigate("/dh/dashboard");
            }
            else if (res.role === "Technician") {
                navigate("/tech/dashboard");
            }
            else {
                navigate("/user");
            }
    } catch (err) {
            const msg =
                err?.response?.data?.message ||
                "Đăng nhập thất bại. Vui lòng thử lại.";
            setMessage(msg);
        } finally {
            setLoading(false);
        }
    };

    // ========================
    // LOGIN GOOGLE
    // ========================
    const handleGoogleSuccess = async (cred) => {
        try {
            const idToken = cred?.credential;
            if (!idToken) return setMessage("Google token không hợp lệ");

            const payload = jwtDecode(idToken);
            const email = payload?.email;

            const res = await authApi.googleLogin({
                credential: idToken,
                email,
            });

            localStorage.setItem("token", res.token);
            localStorage.setItem("role", res.role);

            if (res.role === "Admin") navigate("/admin/dashboard");
            else navigate("/");
        } catch (err) {
            setMessage("Google login thất bại.");
        }
    };

    return (
        <div className="w-screen h-screen bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900">
            <div className="w-full h-full flex flex-col lg:flex-row">

                {/* Bên trái — giới thiệu */}
                <div className="hidden lg:flex flex-1 flex-col justify-center px-16 border-r border-white/10">
                    <div className="max-w-xl">
                        <h1 className="text-4xl font-bold text-white mb-4 tracking-wide">
                            Online Help Desk
                        </h1>
                        <p className="text-slate-200 text-base mb-6">
                            Quản lý yêu cầu nội bộ, phân công kỹ thuật viên, theo dõi SLA.
                        </p>
                        <ul className="space-y-2 text-slate-300 text-sm">
                            <li>• Tạo & theo dõi ticket theo thời gian thực</li>
                            <li>• Giao việc cho Technician theo kỹ năng</li>
                            <li>• Thống kê & báo cáo SLA tự động</li>
                        </ul>
                    </div>
                </div>

                {/* Bên phải — Form Login */}
                <div className="flex-1 flex items-center justify-center px-4 md:px-8 lg:px-16">

                    <div className="w-full max-w-md bg-white/10 backdrop-blur-xl rounded-3xl shadow-2xl p-8 md:p-10 border border-white/20">

                        {/* Logo */}
                        <div className="text-center mb-8">
                            <div className="w-16 h-16 mx-auto bg-indigo-600 rounded-2xl flex items-center justify-center shadow-lg">
                                <span className="text-white font-bold text-2xl">HD</span>
                            </div>

                            <h2 className="text-3xl font-bold text-white mt-4 tracking-wide">
                                Đăng nhập hệ thống
                            </h2>
                            <p className="text-slate-300 mt-1 text-sm">
                                Sử dụng tài khoản nội bộ hoặc Google SSO
                            </p>
                        </div>

                        {message && (
                            <div className="mb-4 text-sm text-center bg-rose-500/20 border border-rose-400/60 text-rose-200 px-4 py-2 rounded-lg">
                                {message}
                            </div>
                        )}

                        {/* Form */}
                        <form className="space-y-4" onSubmit={handleSubmit}>
                            {/* Email */}
                            <div>
                                <label className="text-sm text-slate-200">Email</label>
                                <input
                                    type="email"
                                    required
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    placeholder="you@example.com"
                                    className="w-full bg-white/10 border border-white/20 text-white rounded-xl px-4 py-2.5"
                                />
                            </div>

                            {/* Password */}
                            <div>
                                <label className="text-sm text-slate-200">Mật khẩu</label>
                                <input
                                    type="password"
                                    required
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    placeholder="••••••••"
                                    className="w-full bg-white/10 border border-white/20 text-white rounded-xl px-4 py-2.5"
                                />
                            </div>

                            <div className="flex justify-end text-xs text-indigo-300">
                                <Link to="/forgot-password" className="hover:underline">
                                    Quên mật khẩu?
                                </Link>
                            </div>

                            <button
                                type="submit"
                                disabled={loading}
                                className="w-full bg-indigo-600 hover:bg-indigo-700 text-white rounded-xl py-2.5 font-semibold mt-2 disabled:opacity-60"
                            >
                                {loading ? "Đang xử lý..." : "Đăng nhập"}
                            </button>
                        </form>

                        {/* Divider */}
                        <div className="relative my-6">
                            <div className="absolute inset-0 flex items-center">
                                <span className="w-full border-t border-white/10"></span>
                            </div>
                            <div className="relative flex justify-center text-xs">
                                <span className="bg-slate-900/60 px-3 text-slate-300">
                                    hoặc
                                </span>
                            </div>
                        </div>

                        {/* GOOGLE LOGIN */}
                        <div className="flex justify-center">
                            <GoogleLogin
                                onSuccess={handleGoogleSuccess}
                                onError={() => setMessage("Google login lỗi")}
                                shape="pill"
                                theme="outline"
                                size="large"
                                width="300"
                            />
                        </div>

                    </div>
                </div>
            </div>
        </div>
    );
}
