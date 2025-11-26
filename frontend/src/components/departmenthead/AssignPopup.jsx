import { useEffect, useState } from "react";
import departmentheadApi from "../../api/departmenthead";
import "../../assets/css/departmenthead/Popup.css";

export default function AssignPopup({ requestId, onClose, onAssigned }) {
    const [technicians, setTechnicians] = useState([]);
    const [selectedTech, setSelectedTech] = useState(null);
    const [loading, setLoading] = useState(false);

    // Load Technician list
    useEffect(() => {
        departmentheadApi.getTechnicians().then((res) => {
            setTechnicians(res.data);
        });
    }, []);

    const handleAssign = async () => {
        if (!selectedTech) return;

        setLoading(true);
        try {
            await departmentheadApi.assignRequest({
                requestId,
                assigneeId: selectedTech,
            });

            if (onAssigned) onAssigned();
            onClose();
        } catch (err) {
            alert("Assignment failed");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="dh-popup-overlay">
            <div className="dh-popup">
                <div className="dh-popup-header">
                    <h3>🧑‍🔧 Assign Technician</h3>
                    <button className="dh-popup-close" onClick={onClose}>×</button>
                </div>

                <div className="dh-popup-body">
                    <label className="dh-popup-label">Choose Technician:</label>
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
                        onClick={handleAssign}
                    >
                        {loading ? "Assigning..." : "Assign"}
                    </button>
                </div>
            </div>
        </div>
    );
}
