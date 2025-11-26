import { useEffect, useState } from "react";
import adminApi from "../../api/admin";

export default function AdminUsersPanel() {
    const [users, setUsers] = useState([]);
    const [roles, setRoles] = useState([]);

    // Pagination state
    const [page, setPage] = useState(1);
    const [pageSize] = useState(10);
    const [total, setTotal] = useState(0);

    // Search
    const [search, setSearch] = useState("");

    // Create form
    const [form, setForm] = useState({ email: "", username: "", roleId: "" });
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState("");

    // ============================
    // LOAD USERS WITH PAGINATION
    // ============================
    const loadUsers = async () => {
        try {
            const res = await adminApi.getUsers({
                page,
                pageSize,
                search
            });

            setUsers(res.items || []);
            setTotal(res.total || 0);
        } catch (err) {
            console.error("Load users failed:", err);
        }
    };

    // ============================
    // LOAD ROLES
    // ============================
    const loadRoles = async () => {
        try {
            const data = await adminApi.getRoles();
            setRoles(data || []);
        } catch (err) {
            console.error("Load roles failed:", err);
        }
    };

    useEffect(() => {
        loadUsers();
        loadRoles();
    }, [page, search]);

    // ============================
    // CREATE USER
    // ============================
    const handleCreate = async (e) => {
        e.preventDefault();
        setMessage("");
        setLoading(true);

        try {
            const payload = {
                email: form.email,
                username: form.username || undefined,
                roleId: form.roleId ? Number(form.roleId) : undefined,
            };

            await adminApi.createUser(payload);

            setMessage("User created successfully.");
            setForm({ email: "", username: "", roleId: "" });

            await loadUsers();
        } catch (err) {
            console.error(err);
            setMessage(
                err?.response?.data?.message ||
                err?.response?.data?.error ||
                "Failed to create user."
            );
        } finally {
            setLoading(false);
        }
    };

    // ============================
    // TOGGLE ACTIVE
    // ============================
    const handleToggle = async (u) => {
        try {
            await adminApi.toggleUserActive(u.id, !u.is_Active);
            await loadUsers();
        } catch (err) {
            console.error("Toggle user failed:", err);
        }
    };

    // ============================
    // DELETE USER
    // ============================
    const handleDelete = async (u) => {
        if (!window.confirm(`Delete user ${u.email}?`)) return;

        try {
            await adminApi.deleteUser(u.id);
            await loadUsers();
        } catch (err) {
            console.error("Delete user failed:", err);
        }
    };

    // ============================
    // CHANGE ROLE
    // ============================
    const handleChangeRole = async (id, newRoleId) => {
        try {
            await adminApi.changeUserRole(id, Number(newRoleId));
            await loadUsers();
        } catch (err) {
            console.error("Change role failed:", err);
        }
    };

    // ============================
    // DOWNLOAD EXCEL
    // ============================
    const handleDownloadExcel = async () => {
        try {
            const res = await adminApi.downloadUsersExcel();
            const blob = new Blob([res.data], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            });

            const url = window.URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = "users.xlsx";
            a.click();
            window.URL.revokeObjectURL(url);
        } catch (err) {
            console.error("Download Excel failed:", err);
        }
    };

    // ============================
    // UPLOAD EXCEL
    // ============================
    const handleUploadUpdate = async (e) => {
        const file = e.target.files?.[0];
        if (!file) return;

        try {
            await adminApi.importUsersFromExcelUpdate(file);
            alert("Users updated successfully.");
            await loadUsers();
        } catch (err) {
            console.error("Import Excel failed:", err);
            alert("Failed to import Excel.");
        } finally {
            e.target.value = "";
        }
    };

    return (
        <div className="space-y-6">

            {/* USERS LIST */}
            <div className="admin-panel">
                <div className="admin-panel-header justify-between">
                    <div>
                        <h2 className="admin-panel-title">System Users</h2>
                        <p className="admin-panel-sub">Manage all system accounts.</p>
                    </div>

                    <div className="flex gap-3">
                        <button className="btn-xs btn-outline" onClick={handleDownloadExcel}>
                            ⬇️ Download Excel
                        </button>

                        <div>
                            <input
                                type="file"
                                accept=".xlsx"
                                id="userExcel"
                                className="hidden"
                                onChange={handleUploadUpdate}
                            />
                            <button
                                className="btn-xs btn-outline"
                                onClick={() => document.getElementById("userExcel").click()}
                            >
                                📤 Upload Excel (Update)
                            </button>
                        </div>
                    </div>
                </div>

                {/* SEARCH BAR */}
                <input
                    type="text"
                    placeholder="Search email or username..."
                    className="form-input w-64 mb-3"
                    value={search}
                    onChange={(e) => {
                        setPage(1);
                        setSearch(e.target.value);
                    }}
                />

                <div className="overflow-x-auto">
                    <table className="admin-table">
                        <thead>
                        <tr>
                            <th>Email</th>
                            <th>Username</th>
                            <th>Role</th>
                            <th>Status</th>
                            <th>Actions</th>
                        </tr>
                        </thead>

                        <tbody>
                        {users.length === 0 && (
                            <tr>
                                <td colSpan={5} className="text-center py-6 text-slate-400">
                                    No users found.
                                </td>
                            </tr>
                        )}

                        {users.map((u) => (
                            <tr key={u.id}>
                                <td>{u.email}</td>
                                <td>{u.username}</td>

                                <td>
                                    <select
                                        className="form-input text-xs"
                                        value={u.roleId ?? ""}
                                        onChange={(e) =>
                                            handleChangeRole(u.id, e.target.value)
                                        }
                                    >
                                        <option value="">— None —</option>
                                        {roles.map((r) => (
                                            <option key={r.id} value={r.id}>
                                                {r.name}
                                            </option>
                                        ))}
                                    </select>
                                </td>

                                <td>
                                    <span
                                        className={
                                            "badge " +
                                            (u.is_Active ? "badge-green" : "badge-gray")
                                        }
                                    >
                                        {u.is_Active ? "Active" : "Inactive"}
                                    </span>
                                </td>

                                <td className="space-x-2">
                                    <button
                                        className="btn-xs btn-outline"
                                        onClick={() => handleToggle(u)}
                                    >
                                        {u.is_Active ? "Deactivate" : "Activate"}
                                    </button>

                                    <button
                                        className="btn-xs btn-danger"
                                        onClick={() => handleDelete(u)}
                                    >
                                        Delete
                                    </button>
                                </td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                </div>

                {/* PAGINATION */}
                {/* PAGINATION — đẹp dạng 1,2,3 */}
                <div className="admin-pagination">
                    <div className="admin-pagination-container">

                        {/* Go to first page */}
                        <div
                            className={`admin-page-box ${page === 1 ? "disabled" : ""}`}
                            onClick={() => page !== 1 && setPage(1)}
                        >
                            «
                        </div>

                        {/* Go previous */}
                        <div
                            className={`admin-page-box ${page === 1 ? "disabled" : ""}`}
                            onClick={() => page > 1 && setPage(page - 1)}
                        >
                            ‹
                        </div>

                        {/* Page numbers */}
                        {Array.from({ length: Math.ceil(total / pageSize) }, (_, i) => i + 1)
                            .filter(p =>
                                p === page ||
                                p === page - 1 ||
                                p === page + 1
                            )
                            .map((p) => (
                                <div
                                    key={p}
                                    className={`admin-page-box ${page === p ? "active" : ""}`}
                                    onClick={() => setPage(p)}
                                >
                                    {p}
                                </div>
                            ))}

                        {/* Next */}
                        <div
                            className={`admin-page-box ${
                                page >= Math.ceil(total / pageSize) ? "disabled" : ""
                            }`}
                            onClick={() => page < Math.ceil(total / pageSize) && setPage(page + 1)}
                        >
                            ›
                        </div>

                        {/* Go to last page */}
                        <div
                            className={`admin-page-box ${
                                page >= Math.ceil(total / pageSize) ? "disabled" : ""
                            }`}
                            onClick={() =>
                                page < Math.ceil(total / pageSize) && setPage(Math.ceil(total / pageSize))
                            }
                        >
                            »
                        </div>

                    </div>
                </div>
            </div>
                {/* CREATE USER FORM */}
            <div className="admin-panel">
                <div className="admin-panel-header">
                    <h2 className="admin-panel-title">Create New User</h2>
                </div>

                {message && (
                    <div className="alert mb-3">{message}</div>
                )}

                <form className="space-y-4" onSubmit={handleCreate}>
                    <div>
                        <label className="form-label">Email</label>
                        <input
                            type="email"
                            className="form-input"
                            value={form.email}
                            onChange={(e) =>
                                setForm((prev) => ({ ...prev, email: e.target.value }))
                            }
                            required
                        />
                    </div>

                    <div>
                        <label className="form-label">Username (optional)</label>
                        <input
                            type="text"
                            className="form-input"
                            value={form.username}
                            onChange={(e) =>
                                setForm((prev) => ({ ...prev, username: e.target.value }))
                            }
                        />
                    </div>

                    <div>
                        <label className="form-label">Role</label>
                        <select
                            className="form-input"
                            value={form.roleId}
                            onChange={(e) =>
                                setForm((prev) => ({ ...prev, roleId: e.target.value }))
                            }
                        >
                            <option value="">-- Select role --</option>
                            {roles.map((r) => (
                                <option key={r.id} value={r.id}>
                                    {r.name}
                                </option>
                            ))}
                        </select>
                    </div>

                    <button
                        type="submit"
                        className="btn-primary w-full"
                        disabled={loading}
                    >
                        {loading ? "Creating..." : "Create User"}
                    </button>
                </form>
            </div>
        </div>
    );
}
