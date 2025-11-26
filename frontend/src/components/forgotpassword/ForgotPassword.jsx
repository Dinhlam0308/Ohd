import { useState } from "react";
import { useNavigate } from "react-router-dom";
import authApi from "../../api/auth";

export default function ForgotPassword() {
    const navigate = useNavigate();
    const [email, setEmail] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState(
        "Nhập email, nếu tồn tại chúng tôi sẽ gửi hướng dẫn đặt lại mật khẩu."
    );

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");
        setLoading(true);

        try {
            await authApi.forgotPassword(email);
            setMessage(
                "Nếu email tồn tại trong hệ thống, chúng tôi đã gửi hướng dẫn đặt lại mật khẩu."
            );
        } catch (err) {
            const msg =
                err?.response?.data?.message || "Có lỗi xảy ra. Vui lòng thử lại.";
            setError(msg);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="w-screen h-screen bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900 flex items-center justify-center px-4">
            <div className="w-full max-w-md bg-white/10 backdrop-blur-xl border border-white/20 rounded-3xl shadow-xl p-8">
                {/* Logo */}
                <div className="flex justify-center mb-6">
                    <div className="w-16 h-16 bg-indigo-600 rounded-2xl flex items-center justify-center shadow-lg">
                        <span className="text-white font-bold text-2xl">HD</span>
                    </div>
                </div>

                <h1 className="text-2xl font-bold text-white text-center mb-2">
                    Khôi phục mật khẩu
                </h1>

                <p className="text-slate-300 text-sm text-center mb-6">
                    {message}
                </p>

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-slate-200 mb-1">
                            Email
                        </label>
                        <input
                            type="email"
                            required
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            className="w-full bg-white/10 border border-white/20 text-white rounded-xl px-4 py-2.5 text-sm
                                       placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-indigo-400"
                            placeholder="you@company.com"
                        />
                    </div>

                    {error && (
                        <div className="p-3 bg-rose-500/20 border border-rose-400/60 text-rose-200 text-sm rounded-lg">
                            {error}
                        </div>
                    )}

                    <button
                        type="submit"
                        disabled={loading}
                        className="w-full bg-indigo-600 hover:bg-indigo-700 text-white rounded-xl py-2.5 text-sm font-semibold shadow-lg 
                                   transition disabled:opacity-60"
                    >
                        {loading ? "Đang gửi..." : "Gửi yêu cầu"}
                    </button>
                </form>

                <button
                    onClick={() => navigate("/login")}
                    className="mt-6 w-full text-center text-indigo-300 hover:underline text-sm"
                >
                    ← Quay lại đăng nhập
                </button>
            </div>
        </div>
    );
}
