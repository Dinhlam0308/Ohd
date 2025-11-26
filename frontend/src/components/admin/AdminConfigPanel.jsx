import { useEffect, useState } from "react";
import adminApi from "../../api/admin";

export default function AdminConfigPanel() {
    const [activeTab, setActiveTab] = useState("statuses");
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState("");

    const [statuses, setStatuses] = useState([]);
    const [statusFilter, setStatusFilter] = useState({
        isFinal: "",
        isOverdue: "",
        search: "",
    });

    const [severities, setSeverities] = useState([]);

    const [roles, setRoles] = useState([]);
    const [roleForm, setRoleForm] = useState({
        id: null,
        name: "",
        description: "",
    });

    const [reportRange, setReportRange] = useState("7");
    const [requests, setRequests] = useState([]);
    const [reportStats, setReportStats] = useState({
        total: 0,
        byStatus: [],
    });

    const showMessage = (msg) => {
        setMessage(msg);
        if (!msg) return;
        setTimeout(() => setMessage(""), 3000);
    };

    const loadStatuses = async () => {
        try {
            const filters = {};
            if (statusFilter.search) filters.search = statusFilter.search;
            if (statusFilter.isFinal !== "")
                filters.isFinal = statusFilter.isFinal === "true";
            if (statusFilter.isOverdue !== "")
                filters.isOverdue = statusFilter.isOverdue === "true";

            setStatuses(await adminApi.getRequestStatuses(filters));
        } catch {}
    };

    const loadSeverities = async () => {
        try {
            setSeverities(await adminApi.getSeverities());
        } catch {}
    };

    const loadRoles = async () => {
        try {
            setRoles(await adminApi.getRoles());
        } catch {}
    };

    const loadReportRequests = async () => {
        try {
            setRequests(await adminApi.getRequests({ limit: 1000 }));
        } catch {}
    };

    const recomputeReportStats = () => {
        if (!requests.length) return;

        const now = new Date();
        const days = Number(reportRange) || 7;

        const filtered = requests.filter((r) => {
            if (!r.createdAt) return true;
            const d = new Date(r.createdAt);
            return (now - d) / (1000 * 60 * 60 * 24) <= days;
        });

        const map = new Map();
        filtered.forEach((r) => {
            map.set(r.statusId, (map.get(r.statusId) || 0) + 1);
        });

        setReportStats({
            total: filtered.length,
            byStatus: Array.from(map.entries()).map(([statusId, count]) => ({
                statusId,
                count,
            })),
        });
    };

    useEffect(() => {
        (async () => {
            setLoading(true);
            await Promise.all([loadStatuses(), loadSeverities(), loadRoles()]);
            setLoading(false);
        })();
    }, []);

    useEffect(() => {
        loadStatuses();
    }, [statusFilter.isFinal, statusFilter.isOverdue]);

    useEffect(() => {
        if (activeTab === "reports" && requests.length === 0) {
            (async () => {
                await loadReportRequests();
                recomputeReportStats();
            })();
        }
    }, [activeTab]);

    useEffect(() => {
        if (activeTab === "reports") recomputeReportStats();
    }, [reportRange, requests]);

    /* ========================= ACTIONS ========================= */

    const handleDeleteStatus = async (st) => {
        if (!window.confirm(`Delete status "${st.name}"?`)) return;
        try {
            setLoading(true);
            await adminApi.deleteRequestStatus(st.id);
            await loadStatuses();
            showMessage("Status deleted.");
        } catch {
            showMessage("Cannot delete status.");
        } finally {
            setLoading(false);
        }
    };

    const handleEditRole = (r) =>
        setRoleForm({ id: r.id, name: r.name, description: r.description });

    const handleSubmitRole = async (e) => {
        e.preventDefault();
        try {
            setLoading(true);
            if (roleForm.id) {
                await adminApi.updateRole(roleForm.id, {
                    name: roleForm.name,
                    description: roleForm.description,
                });
            } else {
                await adminApi.createRole({
                    name: roleForm.name,
                    description: roleForm.description,
                });
            }

            await loadRoles();
            showMessage("Saved.");
            setRoleForm({ id: null, name: "", description: "" });
        } catch {
            showMessage("Cannot save role.");
        } finally {
            setLoading(false);
        }
    };

    const handleDeleteRole = async (r) => {
        if (!window.confirm(`Delete role "${r.name}"?`)) return;
        try {
            setLoading(true);
            await adminApi.deleteRole(r.id);
            await loadRoles();
            showMessage("Role deleted.");
        } catch {
            showMessage("Cannot delete role.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="space-y-4">

            {/* HEADER */}
            <div className="flex items-center justify-between">
                <div>
                    <h2 className="glass-header text-xl">System Configuration</h2>
                    <p className="text-xs text-slate-500">
                        Manage statuses, severities, roles & reports.
                    </p>
                </div>
                {loading && (
                    <span className="text-xs text-slate-500 animate-pulse">
                        Loading...
                    </span>
                )}
            </div>

            {/* TABS */}
            <div className="flex gap-2 text-xs">
                {[
                    { id: "statuses", label: "Statuses" },
                    { id: "severities", label: "Severities" },
                    { id: "roles", label: "Roles" },
                    { id: "reports", label: "Reports" },
                ].map((t) => (
                    <button
                        key={t.id}
                        onClick={() => setActiveTab(t.id)}
                        className={`glass-tab ${
                            activeTab === t.id
                                ? "glass-tab-active"
                                : "glass-tab-inactive"
                        }`}
                    >
                        {t.label}
                    </button>
                ))}
            </div>

            {/* TOAST */}
            {message && <div className="glass-toast">{message}</div>}

            {/* MAIN PANEL */}
            <div className="glass-panel p-6 space-y-6">

                {/* ================== STATUS ================== */}
                {activeTab === "statuses" && (
                    <div>
                        <h3 className="text-sm text-slate-800 mb-2">
                            Request statuses
                        </h3>

                        <div className="mb-3 flex gap-3">
                            <input
                                type="text"
                                placeholder="Search..."
                                className="glass-input"
                                value={statusFilter.search}
                                onChange={(e) =>
                                    setStatusFilter({
                                        ...statusFilter,
                                        search: e.target.value,
                                    })
                                }
                                onBlur={loadStatuses}
                            />

                            <select
                                className="glass-input"
                                value={statusFilter.isFinal}
                                onChange={(e) =>
                                    setStatusFilter({
                                        ...statusFilter,
                                        isFinal: e.target.value,
                                    })
                                }
                            >
                                <option value="">Final: All</option>
                                <option value="true">Final</option>
                                <option value="false">Non-final</option>
                            </select>

                            <select
                                className="glass-input"
                                value={statusFilter.isOverdue}
                                onChange={(e) =>
                                    setStatusFilter({
                                        ...statusFilter,
                                        isOverdue: e.target.value,
                                    })
                                }
                            >
                                <option value="">Overdue: All</option>
                                <option value="true">Overdue</option>
                                <option value="false">Normal</option>
                            </select>
                        </div>

                        <table className="glass-table">
                            <thead>
                            <tr>
                                <th>Code</th>
                                <th>Name</th>
                                <th className="text-center">Final</th>
                                <th className="text-center">Overdue</th>
                                <th className="text-right">Actions</th>
                            </tr>
                            </thead>

                            <tbody>
                            {statuses.map((st) => (
                                <tr key={st.id}>
                                    <td>{st.code}</td>
                                    <td>{st.name}</td>
                                    <td className="text-center">
                                        {st.isFinal ? "Yes" : "No"}
                                    </td>
                                    <td className="text-center">
                                        {st.isOverdue ? "Yes" : "No"}
                                    </td>
                                    <td className="text-right space-x-2">
                                        <button
                                            className="glass-btn glass-btn-danger"
                                            onClick={() => handleDeleteStatus(st)}
                                        >
                                            Delete
                                        </button>
                                    </td>
                                </tr>
                            ))}
                            </tbody>
                        </table>
                    </div>
                )}

                {/* ================== SEVERITIES ================== */}
                {activeTab === "severities" && (
                    <div className="grid grid-cols-1 gap-6">

                        <table className="glass-table">
                            <thead>
                            <tr>
                                <th>Code</th>
                                <th>Name</th>
                                <th className="text-center">Order</th>
                                <th className="text-right">Actions</th>
                            </tr>
                            </thead>

                            <tbody>
                            {severities.map((s) => (
                                <tr key={s.id}>
                                    <td>{s.code}</td>
                                    <td>{s.name}</td>
                                    <td className="text-center">{s.sort_order}</td>
                                    <td className="text-right space-x-2">
                                        <button
                                            className="glass-btn glass-btn-danger"
                                            onClick={() => handleSevDelete(s)}
                                        >
                                            Delete
                                        </button>
                                    </td>
                                </tr>
                            ))}
                            </tbody>
                        </table>

                    </div>
                )}

                {/* ================== ROLES ================== */}
                {activeTab === "roles" && (
                    <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">

                        <table className="glass-table">
                            <thead>
                            <tr>
                                <th>ID</th>
                                <th>Name</th>
                                <th>Description</th>
                                <th className="text-right">Actions</th>
                            </tr>
                            </thead>

                            <tbody>
                            {roles.map((r) => (
                                <tr key={r.id}>
                                    <td>{r.id}</td>
                                    <td>{r.name}</td>
                                    <td>{r.description || "—"}</td>
                                    <td className="text-right space-x-2">
                                        <button
                                            className="glass-btn glass-btn-light"
                                            onClick={() => handleEditRole(r)}
                                        >
                                            Edit
                                        </button>
                                        <button
                                            className="glass-btn glass-btn-danger"
                                            onClick={() => handleDeleteRole(r)}
                                        >
                                            Delete
                                        </button>
                                    </td>
                                </tr>
                            ))}
                            </tbody>
                        </table>

                        <div className="glass-panel p-4 space-y-3">
                            <h3 className="text-sm glass-header mb-3">
                                {roleForm.id ? "Update role" : "Create role"}
                            </h3>

                            <form className="space-y-3" onSubmit={handleSubmitRole}>
                                <div>
                                    <label className="text-slate-700 text-xs">Role name</label>
                                    <input
                                        type="text"
                                        className="glass-input"
                                        value={roleForm.name}
                                        onChange={(e) =>
                                            setRoleForm({ ...roleForm, name: e.target.value })
                                        }
                                        required
                                    />
                                </div>

                                <div>
                                    <label className="text-slate-700 text-xs">Description</label>
                                    <textarea
                                        rows={3}
                                        className="glass-input"
                                        value={roleForm.description}
                                        onChange={(e) =>
                                            setRoleForm({
                                                ...roleForm,
                                                description: e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <button className="glass-btn glass-btn-primary w-full">
                                    {roleForm.id ? "Update" : "Create"}
                                </button>
                            </form>
                        </div>

                    </div>
                )}

                {/* ================== REPORTS ================== */}
                {activeTab === "reports" && (
                    <div>
                        <h3 className="text-sm glass-header mb-3">Reports</h3>

                        <div className="flex gap-3 mb-3">
                            <select
                                className="glass-input w-auto"
                                value={reportRange}
                                onChange={(e) => setReportRange(e.target.value)}
                            >
                                <option value="7">Last 7 days</option>
                                <option value="30">Last 30 days</option>
                                <option value="90">Last 90 days</option>
                            </select>

                            <span className="text-slate-700 text-xs">
                                Total requests:{" "}
                                <span className="text-slate-900 font-semibold">
                                    {reportStats.total}
                                </span>
                            </span>
                        </div>

                        {reportStats.byStatus.map((item) => (
                            <div
                                key={item.statusId}
                                className="glass-panel p-4 flex justify-between mb-2"
                            >
                                <span className="text-slate-700">
                                    Status {item.statusId}
                                </span>
                                <span className="text-slate-900 font-semibold">
                                    {item.count}
                                </span>
                            </div>
                        ))}
                    </div>
                )}

            </div>
        </div>
    );
}
