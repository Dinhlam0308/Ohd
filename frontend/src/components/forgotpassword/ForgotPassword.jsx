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
        <div className="min-h-screen bg-[#0b1526] flex items-center justify-center px-4 py-8">
            <div className="bg-white rounded-2xl shadow-2xl p-8 w-full max-w-xl">
                <h1 className="text-xl font-bold text-slate-900 mb-4">
                    Khôi phục mật khẩu
                </h1>
                <p className="text-slate-500 text-sm mb-4">{message}</p>

                <form onSubmit={handleSubmit} className="space-y-4">
                    <input
                        type="email"
                        required
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        className="w-full rounded-lg border px-3 py-2"
                        placeholder="you@company.com"
                    />

                    {error && (
                        <div className="p-3 bg-rose-100 border border-rose-400 text-rose-700 text-sm rounded-lg">
                            {error}
                        </div>
                    )}

                    <button
                        type="submit"
                        disabled={loading}
                        className="w-full bg-indigo-600 text-white py-2 rounded-lg"
                    >
                        {loading ? "Đang gửi..." : "Gửi yêu cầu"}
                    </button>
                </form>

                <button
                    onClick={() => navigate("/login")}
                    className="mt-4 text-xs text-indigo-600 hover:underline"
                >
                    Quay lại đăng nhập
                </button>
            </div>
        </div>
    );
}
