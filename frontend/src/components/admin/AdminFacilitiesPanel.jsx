// src/components/admin/AdminFacilitiesPanel.jsx
import { useEffect, useState } from "react";
import adminApi from "../../api/admin";

export default function AdminFacilitiesPanel() {
    const [facilities, setFacilities] = useState([]);
    const [technicians, setTechnicians] = useState([]);

    const [search, setSearch] = useState("");

    // Pagination
    const [page, setPage] = useState(1);
    const [pageSize] = useState(10);
    const [total, setTotal] = useState(0);

    const [loading, setLoading] = useState(false);

    const [form, setForm] = useState({
        id: null,
        name: "",
        description: "",
        headUserId: "",
    });

    const [message, setMessage] = useState("");
    const [messageType, setMessageType] = useState("");

    // ==============================================
    // LOAD FACILITIES (Có search + pagination)
    // ==============================================
    const loadFacilities = async () => {
        try {
            const res = await adminApi.getFacilities({
                page,
                pageSize,
                search,
            });

            setFacilities(res.items || []);
            setTotal(res.total || 0);
        } catch (err) {
            console.error("Failed to load facilities:", err);
        }
    };

    // ==============================================
    // LOAD TECHNICIANS
    // ==============================================
    const loadTechnicians = async () => {
        try {
            const data = await adminApi.getTechnicians();
            setTechnicians(data || []);
        } catch (err) {
            console.error("Failed to load technicians:", err);
        }
    };

    useEffect(() => {
        loadFacilities();
        loadTechnicians();
    }, [page, search]);

    const resetForm = () => {
        setForm({
            id: null,
            name: "",
            description: "",
            headUserId: "",
        });
        setMessage("");
        setMessageType("");
    };

    const handleEdit = (f) => {
        setForm({
            id: f.id,
            name: f.name,
            description: f.description || "",
            headUserId: f.headUserId || "",
        });
    };

    const handleDelete = async (f) => {
        if (!window.confirm(`Delete facility "${f.name}"?`)) return;

        try {
            await adminApi.deleteFacility(f.id);
            setMessage("Facility deleted successfully.");
            setMessageType("success");
            loadFacilities();
            resetForm();
        } catch (err) {
            console.error(err);
            setMessage("Unable to delete facility.");
            setMessageType("error");
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage("");
        setLoading(true);

        if (!form.headUserId) {
            setMessage("Technician cannot be empty.");
            setMessageType("error");
            setLoading(false);
            return;
        }

        const payload = {
            name: form.name,
            description: form.description,
            headUserId: Number(form.headUserId),
        };

        try {
            if (form.id) {
                await adminApi.updateFacility(form.id, payload);
                setMessage("Facility updated successfully.");
            } else {
                await adminApi.createFacility(payload);
                setMessage("Facility created successfully.");
            }

            setMessageType("success");
            loadFacilities();
            resetForm();
        } catch (err) {
            console.error(err);
            setMessage("Unable to save facility.");
            setMessageType("error");
        } finally {
            setLoading(false);
        }
    };

    // Pagination numbers
    const totalPages = Math.ceil(total / pageSize);
    const pages = Array.from({ length: totalPages }, (_, i) => i + 1);

    return (
        <div className="grid grid-cols-1 xl:grid-cols-[2fr,1.2fr] gap-6">

            {/* FACILITY LIST */}
            <div className="glass-panel p-6">

                <div className="admin-panel-header mb-4">
                    <h2 className="glass-header">Facilities</h2>
                    <p className="admin-panel-sub">
                        Manage facility list & assigned Technician.
                    </p>
                </div>

                {/* SEARCH BAR */}
                <div className="mb-4">
                    <input
                        type="text"
                        placeholder="🔍 Search facility..."
                        className="glass-input w-64"
                        value={search}
                        onChange={(e) => {
                            setPage(1); // reset page
                            setSearch(e.target.value);
                        }}
                    />
                </div>

                <div className="overflow-x-auto">
                    <table className="glass-table">
                        <thead>
                        <tr>
                            <th>Name</th>
                            <th>Description</th>
                            <th>Technician</th>
                            <th>Actions</th>
                        </tr>
                        </thead>

                        <tbody>
                        {facilities.length === 0 && (
                            <tr>
                                <td colSpan={4} className="text-center py-6 text-slate-400">
                                    No facilities found.
                                </td>
                            </tr>
                        )}

                        {facilities.map((f) => (
                            <tr key={f.id}>
                                <td>{f.name}</td>
                                <td>{f.description || "—"}</td>
                                <td>
                                    {technicians.find((t) => t.id === f.headUserId)?.username || "—"}
                                </td>
                                <td className="space-x-2">
                                    <button
                                        className="glass-btn glass-btn-light"
                                        onClick={() => handleEdit(f)}
                                    >
                                        Edit
                                    </button>
                                    <button
                                        className="glass-btn glass-btn-danger"
                                        onClick={() => handleDelete(f)}
                                    >
                                        Delete
                                    </button>
                                </td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                </div>

                {/* PAGINATION CENTERED */}
                <div className="flex justify-center gap-2 mt-5">

                    <button
                        className="glass-btn glass-btn-light"
                        disabled={page === 1}
                        onClick={() => setPage(page - 1)}
                    >
                        «
                    </button>

                    {pages.map((p) => (
                        <button
                            key={p}
                            className={`glass-btn ${
                                p === page
                                    ? "glass-btn-primary"
                                    : "glass-btn-light"
                            }`}
                            onClick={() => setPage(p)}
                        >
                            {p}
                        </button>
                    ))}

                    <button
                        className="glass-btn glass-btn-light"
                        disabled={page >= totalPages}
                        onClick={() => setPage(page + 1)}
                    >
                        »
                    </button>
                </div>
            </div>

            {/* FORM */}
            <div className="glass-panel p-6">
                <h2 className="glass-header mb-3">
                    {form.id ? "Update Facility" : "Create Facility"}
                </h2>

                {message && (
                    <div
                        className={`mb-3 text-sm font-semibold ${
                            messageType === "error"
                                ? "text-red-400"
                                : "text-green-400"
                        }`}
                    >
                        {message}
                    </div>
                )}

                <form className="space-y-4" onSubmit={handleSubmit}>
                    <div>
                        <label className="form-label">Facility Name</label>
                        <input
                            type="text"
                            className="glass-input"
                            value={form.name}
                            onChange={(e) => setForm({ ...form, name: e.target.value })}
                            required
                        />
                    </div>

                    <div>
                        <label className="form-label">Description</label>
                        <textarea
                            className="glass-input"
                            rows={3}
                            value={form.description}
                            onChange={(e) =>
                                setForm({ ...form, description: e.target.value })
                            }
                        />
                    </div>

                    <div>
                        <label className="form-label">Technician *</label>
                        <select
                            className="glass-input"
                            value={form.headUserId}
                            onChange={(e) =>
                                setForm({ ...form, headUserId: e.target.value })
                            }
                            required
                        >
                            <option value="">-- Select Technician --</option>
                            {technicians.map((t) => (
                                <option key={t.id} value={t.id}>
                                    {t.username || t.email}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className="flex gap-3">
                        <button className="glass-btn glass-btn-primary flex-1" disabled={loading}>
                            {loading ? "Saving..." : form.id ? "Update" : "Create"}
                        </button>

                        {form.id && (
                            <button
                                type="button"
                                className="glass-btn glass-btn-light"
                                onClick={resetForm}
                            >
                                Cancel
                            </button>
                        )}
                    </div>
                </form>
            </div>
        </div>
    );
}
