// src/components/departmenthead/ReassignPopup.jsx
import { useEffect, useState } from "react";
import departmentheadApi from "../../api/departmenthead";

export default function ReassignPopup({ requestId, onClose, onReassigned }) {
    const [technicians, setTechnicians] = useState([]);
    const [selectedTech, setSelectedTech] = useState(null);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        departmentheadApi.getTechnicians().then((res) => {
            setTechnicians(res.data || []);
        });
    }, []);

    const handleReassign = async () => {
        if (!selectedTech) return;
        setLoading(true);
        try {
            await departmentheadApi.assignRequest({
                requestId,
                assigneeId: selectedTech,
            });
            if (onReassigned) onReassigned();
            onClose();
        } catch (err) {
            alert("Reassign failed");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="dh-popup-overlay">
            <div className="dh-popup">
                <div className="dh-popup-header">
                    <h3>🔁 Reassign Technician</h3>
                    <button className="dh-popup-close" onClick={onClose}>
                        ×
                    </button>
                </div>

                <div className="dh-popup-body">
                    <label className="dh-popup-label">Choose new technician:</label>
                    <select
                        className="dh-popup-select"
                        value={selectedTech || ""}
                        onChange={(e) => setSelectedTech(e.target.value)}
                    >
                        <option value="">-- Select technician --</option>
                        {technicians.map((t) => (
                            <option key={t.id} value={t.id}>
                                {t.username || t.email}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="dh-popup-footer">
                    <button className="dh-btn-secondary" onClick={onClose}>
                        Cancel
                    </button>
                    <button
                        className="dh-btn-primary"
                        disabled={loading || !selectedTech}
                        onClick={handleReassign}
                    >
                        {loading ? "Reassigning..." : "Reassign"}
                    </button>
                </div>
            </div>
        </div>
    );
}
