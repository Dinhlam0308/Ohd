import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import authApi from "../../api/auth";

export default function ChangePasswordFirstLogin() {
    const navigate = useNavigate();
    const [oldPassword, setOldPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [message, setMessage] = useState("");
    const [loading, setLoading] = useState(false);

    // Lấy userId từ token (decode JWT)
    const [userId, setUserId] = useState(null);

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (!token) {
            navigate("/login");
        } else {
            try {
                const payload = JSON.parse(atob(token.split(".")[1]));
                setUserId(payload.sub); // lấy userId
            } catch {
                navigate("/login");
            }
        }
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage("");

        if (newPassword !== confirmPassword) {
            setMessage("Mật khẩu mới không khớp.");
            return;
        }

        setLoading(true);

        try {
            await authApi.changePasswordFirstLogin({
                userId,
                oldPassword,
                newPassword,
                confirmNewPassword: confirmPassword,
            });

            localStorage.removeItem("require_change_password");
            alert("Đổi mật khẩu thành công! Vui lòng đăng nhập lại.");
            navigate("/login");
        } catch (err) {
            const msg =
                err?.response?.data?.message ||
                "Đổi mật khẩu thất bại.";
            setMessage(msg);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-slate-900 to-slate-800 p-4">
            <div className="bg-white/10 backdrop-blur-xl border border-white/20 rounded-2xl p-8 w-full max-w-md shadow-xl">
                <h2 className="text-2xl font-bold text-white text-center mb-4">
                    Đổi mật khẩu lần đầu
                </h2>
                <p className="text-slate-300 text-sm text-center mb-6">
                    Bạn cần đặt mật khẩu mới để tiếp tục sử dụng hệ thống.
                </p>

                {message && (
                    <div className="mb-4 text-sm text-center bg-rose-500/20 border border-rose-400/60 text-rose-200 px-4 py-2 rounded-lg">
                        {message}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="text-sm text-slate-200">Mật khẩu cũ</label>
                        <input
                            type="password"
                            value={oldPassword}
                            onChange={(e) => setOldPassword(e.target.value)}
                            required
                            className="w-full bg-white/10 border border-white/20 text-white rounded-lg px-4 py-2"
                        />
                    </div>

                    <div>
                        <label className="text-sm text-slate-200">Mật khẩu mới</label>
                        <input
                            type="password"
                            value={newPassword}
                            onChange={(e) => setNewPassword(e.target.value)}
                            required
                            className="w-full bg-white/10 border border-white/20 text-white rounded-lg px-4 py-2"
                        />
                    </div>

                    <div>
                        <label className="text-sm text-slate-200">
                            Nhập lại mật khẩu mới
                        </label>
                        <input
                            type="password"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            required
                            className="w-full bg-white/10 border border-white/20 text-white rounded-lg px-4 py-2"
                        />
                    </div>

                    <button
                        type="submit"
                        disabled={loading}
                        className="w-full bg-indigo-600 hover:bg-indigo-700 text-white rounded-lg py-2.5 font-semibold mt-2 disabled:opacity-60"
                    >
                        {loading ? "Đang xử lý..." : "Đổi mật khẩu"}
                    </button>
                </form>
            </div>
        </div>
    );
}
