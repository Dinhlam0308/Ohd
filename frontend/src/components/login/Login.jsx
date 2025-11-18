// src/pages/Auth/LoginPage.jsx
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

    // ==========================
    // LOGIN EMAIL + PASSWORD
    // ==========================
    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage("");
        setLoading(true);

        try {
            const data = await authApi.login({ email, password });
            // data: { token, requireChangePassword }

            localStorage.setItem("token", data.token);

            if (data.requireChangePassword) {
                localStorage.setItem("require_change_password", "true");
                navigate("/change-password-first-login");
            } else {
                localStorage.removeItem("require_change_password");
                navigate("/");
            }
        } catch (err) {
            console.error("Login error:", err);
            const msg =
                err?.response?.data?.message ||
                "Đăng nhập thất bại. Vui lòng thử lại.";
            setMessage(msg);
        } finally {
            setLoading(false);
        }
    };

    // ==========================
    // LOGIN GOOGLE
    // ==========================
    const handleGoogleSuccess = async (cred) => {
        try {
            setMessage("");
            setLoading(true);

            const idToken = cred?.credential;
            if (!idToken) {
                setMessage("Token Google không hợp lệ.");
                return;
            }

            const payload = jwtDecode(idToken);
            const emailFromGoogle = payload?.email;

            const res = await authApi.googleLogin({
                credential: idToken,
                email: emailFromGoogle,
            });
            // res: { token }

            localStorage.setItem("token", res.token);
            localStorage.removeItem("require_change_password");

            navigate("/");
        } catch (err) {
            console.error("Google login error:", err);
            const msg =
                err?.response?.data?.message ||
                "Google login thất bại. Vui lòng thử lại.";
            setMessage(msg);
        } finally {
            setLoading(false);
        }
    };

    const handleGoogleError = () => {
        setMessage("Google login thất bại. Vui lòng thử lại.");
    };

    return (
        <div className="w-screen h-screen bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900">
            <div className="w-full h-full flex flex-col lg:flex-row">
                {/* Bên trái: giới thiệu (ẩn trên mobile) */}
                <div className="hidden lg:flex flex-1 flex-col justify-center px-16 border-r border-white/10">
                    <div className="max-w-xl">
                        <h1 className="text-4xl font-bold text-white mb-4 tracking-wide">
                            Online Help Desk
                        </h1>
                        <p className="text-slate-200 text-base mb-6">
                            Quản lý yêu cầu nội bộ, phân công kỹ thuật viên, theo dõi SLA và
                            trạng thái xử lý một cách tập trung và chuyên nghiệp.
                        </p>
                        <ul className="space-y-2 text-slate-300 text-sm">
                            <li>• Tạo & theo dõi ticket theo thời gian thực</li>
                            <li>• Giao việc cho Technician theo kỹ năng & đội nhóm</li>
                            <li>• Thống kê, báo cáo SLA và hiệu suất xử lý</li>
                        </ul>
                    </div>
                </div>

                {/* Bên phải: form login */}
                <div className="flex-1 flex items-center justify-center px-4 md:px-8 lg:px-16">
                    <div className="w-full max-w-md bg-white/10 backdrop-blur-xl rounded-3xl shadow-2xl p-8 md:p-10 border border-white/20">
                        {/* Logo + title */}
                        <div className="text-center mb-8">
                            <div className="w-16 h-16 mx-auto bg-indigo-600 rounded-2xl flex items-center justify-center shadow-lg">
                                <span className="text-white font-bold text-2xl">HD</span>
                            </div>

                            <h2 className="text-2xl md:text-3xl font-bold text-white mt-4 tracking-wide">
                                Đăng nhập hệ thống
                            </h2>
                            <p className="text-slate-300 mt-1 text-sm">
                                Sử dụng tài khoản nội bộ hoặc Google SSO
                            </p>
                        </div>

                        {/* Thông báo lỗi */}
                        {message && (
                            <div className="mb-4 text-sm text-center bg-rose-500/20 border border-rose-400/60 text-rose-200 px-4 py-2 rounded-lg">
                                {message}
                            </div>
                        )}

                        {/* Form login */}
                        <form className="space-y-4" onSubmit={handleSubmit}>
                            {/* Email */}
                            <div>
                                <label className="block text-sm font-medium text-slate-200 mb-1">
                                    Email
                                </label>
                                <input
                                    type="email"
                                    className="w-full bg-white/10 border border-white/20 text-white rounded-xl px-4 py-2.5 text-sm
                             placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-indigo-400"
                                    placeholder="you@example.com"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                />
                            </div>

                            {/* Password */}
                            <div>
                                <label className="block text-sm font-medium text-slate-200 mb-1">
                                    Mật khẩu
                                </label>
                                <input
                                    type="password"
                                    className="w-full bg-white/10 border border-white/20 text-white rounded-xl px-4 py-2.5 text-sm
                             placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-indigo-400"
                                    placeholder="••••••••"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                />
                            </div>

                            {/* Quên mật khẩu */}
                            <div className="flex items-center justify-between text-xs md:text-sm text-slate-300 mt-1">
                                <div />
                                <Link
                                    className="hover:underline text-indigo-300"
                                    to="/forgot-password"
                                >
                                    Quên mật khẩu?
                                </Link>
                            </div>

                            {/* Nút login */}
                            <button
                                type="submit"
                                className="w-full bg-indigo-600 hover:bg-indigo-700 text-white rounded-xl py-2.5 text-sm font-semibold shadow-lg 
                           transition disabled:opacity-60 disabled:cursor-not-allowed mt-2"
                                disabled={loading}
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
                  hoặc đăng nhập bằng
                </span>
                            </div>
                        </div>

                        {/* Google Login */}
                        <div className="flex justify-center">
                            <GoogleLogin
                                onSuccess={handleGoogleSuccess}
                                onError={handleGoogleError}
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
