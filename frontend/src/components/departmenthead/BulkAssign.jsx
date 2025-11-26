// src/components/departmenthead/BulkAssign.jsx
import { useEffect, useState } from "react";
import departmentheadApi from "../../api/departmenthead";

export default function BulkAssign() {
    const [requests, setRequests] = useState([]);
    const [technicians, setTechnicians] = useState([]);
    const [selectedIds, setSelectedIds] = useState([]);
    const [assigneeId, setAssigneeId] = useState("");
    const [loading, setLoading] = useState(false);

    const load = async () => {
        const [reqRes, techRes] = await Promise.all([
            departmentheadApi.getBulkRequests(),
            departmentheadApi.getTechnicians(),
        ]);
        setRequests(reqRes.data || []);
        setTechnicians(techRes.data || []);
    };

    useEffect(() => {
        load();
    }, []);

    const toggleOne = (id) => {
        setSelectedIds((prev) =>
            prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id]
        );
    };

    const toggleAll = () => {
        if (selectedIds.length === requests.length) {
            setSelectedIds([]);
        } else {
            setSelectedIds(requests.map((r) => r.id));
        }
    };

    const handleBulkAssign = async () => {
        if (!assigneeId || selectedIds.length === 0) return;
        setLoading(true);
        try {
            await departmentheadApi.bulkAssign({
                requestIds: selectedIds,
                assigneeId: assigneeId,
            });
            alert("Bulk assign successful");
            setSelectedIds([]);
            load();
        } catch (err) {
            alert("Bulk assign failed");
        } finally {
            setLoading(false);
        }
    };

    return (
        <>
            <div className="dh-page-title">📦 Bulk Assign</div>
            <div className="dh-page-sub">
                Select multiple requests and assign them to a technician at once.
            </div>

            <div className="dh-card" style={{ marginBottom: 18 }}>
                <div className="dh-export-form">
                    <div className="field">
                        <label>Assign to</label>
                        <select
                            value={assigneeId}
                            onChange={(e) => setAssigneeId(e.target.value)}
                        >
                            <option value="">-- Select technician --</option>
                            {technicians.map((t) => (
                                <option key={t.id} value={t.id}>
                                    {t.username || t.email}
                                </option>
                            ))}
                        </select>
                    </div>

                    <button
                        className="dh-btn-primary"
                        disabled={loading || !assigneeId || selectedIds.length === 0}
                        onClick={handleBulkAssign}
                    >
                        {loading ? "Assigning..." : "Assign selected"}
                    </button>
                </div>
            </div>

            <div className="dh-table-wrapper">
                <table className="dh-table">
                    <thead>
                    <tr>
                        <th>
                            <input
                                type="checkbox"
                                checked={
                                    selectedIds.length === requests.length &&
                                    requests.length > 0
                                }
                                onChange={toggleAll}
                            />
                        </th>
                        <th>Request</th>
                        <th>Severity</th>
                        <th>Created</th>
                    </tr>
                    </thead>
                    <tbody>
                    {requests.map((r) => (
                        <tr key={r.id}>
                            <td>
                                <input
                                    type="checkbox"
                                    checked={selectedIds.includes(r.id)}
                                    onChange={() => toggleOne(r.id)}
                                />
                            </td>
                            <td>{r.title}</td>
                            <td>{r.severityName || r.severityId}</td>
                            <td>
                                {r.createdAt &&
                                    new Date(r.createdAt).toLocaleString()}
                            </td>
                        </tr>
                    ))}

                    {requests.length === 0 && (
                        <tr>
                            <td colSpan={4} className="dh-empty">
                                No requests available for bulk assignment.
                            </td>
                        </tr>
                    )}
                    </tbody>
                </table>
            </div>
        </>
    );
}
