// src/components/admin/AdminFacilitiesPanel.jsx
import { useEffect, useState } from "react";
import adminApi from "../../api/admin";

export default function AdminFacilitiesPanel() {
    const [facilities, setFacilities] = useState([]);
    const [technicians, setTechnicians] = useState([]);
    const [loading, setLoading] = useState(false);

    const [form, setForm] = useState({
        id: null,
        name: "",
        description: "",
        headUserId: "",
    });

    const [message, setMessage] = useState("");
    const [messageType, setMessageType] = useState(""); // success | error

    // ==============================================
    // LOAD FACILITIES
    // ==============================================
    const loadFacilities = async () => {
        try {
            const data = await adminApi.getFacilities();
            setFacilities(data || []);
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
    }, []);

    // ==============================================
    // RESET FORM
    // ==============================================
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

    // ==============================================
    // EDIT
    // ==============================================
    const handleEdit = (f) => {
        setForm({
            id: f.id,
            name: f.name,
            description: f.description || "",
            headUserId: f.headUserId || "",
        });
    };

    // ==============================================
    // DELETE
    // ==============================================
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

    // ==============================================
    // CREATE / UPDATE
    // ==============================================
    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage("");
        setLoading(true);

        // ❌ Validate: Technician is required
        if (!form.headUserId || form.headUserId === "") {
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

    return (
        <div className="grid grid-cols-1 xl:grid-cols-[2fr,1.2fr] gap-6">

            {/* FACILITY LIST */}
            <div className="admin-panel">
                <div className="admin-panel-header">
                    <h2 className="admin-panel-title">Facilities</h2>
                    <p className="admin-panel-sub">
                        Manage facility list and assigned Technician.
                    </p>
                </div>

                <div className="overflow-x-auto">
                    <table className="admin-table">
                        <thead>
                        <tr>
                            <th>Facility Name</th>
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
                                <td className="text-sm text-black">{f.name}</td>
                                <td className="text-sm text-slate-700">
                                    {f.description || "—"}
                                </td>
                                <td className="text-sm text-slate-900">
                                    {technicians.find((t) => t.id === f.headUserId)?.username || "—"}
                                </td>

                                <td className="space-x-2">
                                    <button
                                        className="btn-xs btn-outline"
                                        onClick={() => handleEdit(f)}
                                    >
                                        Edit
                                    </button>

                                    <button
                                        className="btn-xs btn-danger"
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
            </div>

            {/* FORM */}
            <div className="admin-panel">
                <div className="admin-panel-header">
                    <h2 className="admin-panel-title">
                        {form.id ? "Update Facility" : "Create New Facility"}
                    </h2>
                </div>

                {/* MESSAGE */}
                {message && (
                    <div
                        className={`mb-3 text-sm font-semibold ${
                            messageType === "error" ? "text-red-400" : "text-green-400"
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
                            className="form-input"
                            value={form.name}
                            onChange={(e) => setForm({ ...form, name: e.target.value })}
                            required
                        />
                    </div>

                    <div>
                        <label className="form-label">Description</label>
                        <textarea
                            className="form-input"
                            rows={3}
                            value={form.description}
                            onChange={(e) =>
                                setForm({ ...form, description: e.target.value })
                            }
                        />
                    </div>

                    {/* TECHNICIAN REQUIRED FIELD */}
                    <div>
                        <label className="form-label">Technician *</label>
                        <select
                            className={`form-input ${!form.headUserId ? "border-red-500" : ""}`}
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

                        {!form.headUserId && (
                            <p className="text-xs text-red-500 mt-1">
                                Technician cannot be empty.
                            </p>
                        )}
                    </div>

                    <div className="flex gap-3">
                        <button className="btn-primary flex-1" disabled={loading}>
                            {loading ? "Saving..." : form.id ? "Update" : "Create"}
                        </button>

                        {form.id && (
                            <button
                                type="button"
                                className="btn-outline"
                                onClick={resetForm}
                            >
                                Cancel / New
                            </button>
                        )}
                    </div>
                </form>
            </div>
        </div>
    );
}
