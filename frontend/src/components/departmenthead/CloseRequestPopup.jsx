// src/components/departmenthead/CloseRequestPopup.jsx
import { useState } from "react";
import departmentheadApi from "../../api/departmenthead";

export default function CloseRequestPopup({ requestId, onClose, onClosed }) {
    const [note, setNote] = useState("");
    const [loading, setLoading] = useState(false);

    const handleClose = async () => {
        setLoading(true);
        try {
            // Bạn có thể tạo endpoint riêng: /departmenthead/request/{id}/close
            await departmentheadApi.changeStatus?.(requestId, {
                status: "Completed",
                note,
            });
            if (onClosed) onClosed();
            onClose();
        } catch (err) {
            alert("Close request failed");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="dh-popup-overlay">
            <div className="dh-popup">
                <div className="dh-popup-header">
                    <h3>✅ Close Request</h3>
                    <button className="dh-popup-close" onClick={onClose}>
                        ×
                    </button>
                </div>

                <div className="dh-popup-body">
                    <label className="dh-popup-label">Closing note (optional):</label>
                    <textarea
                        className="dh-popup-select"
                        style={{ minHeight: 80 }}
                        value={note}
                        onChange={(e) => setNote(e.target.value)}
                    />
                </div>

                <div className="dh-popup-footer">
                    <button className="dh-btn-secondary" onClick={onClose}>
                        Cancel
                    </button>
                    <button
                        className="dh-btn-primary"
                        disabled={loading}
                        onClick={handleClose}
                    >
                        {loading ? "Closing..." : "Close request"}
                    </button>
                </div>
            </div>
        </div>
    );
}
