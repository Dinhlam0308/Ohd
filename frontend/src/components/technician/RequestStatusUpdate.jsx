import { useState } from "react";
import technicianApi from "../../api/technician";

export default function RequestStatusUpdate({ requestId, onUpdated }) {
    const [statusId, setStatusId] = useState("");
    const [note, setNote] = useState("");

    const handleUpdate = async () => {
        try {
            await technicianApi.updateStatus(requestId, { statusId, note });
            alert("Status updated!");

            onUpdated();
        } catch (err) {
            console.error(err);
            alert("Failed to update status");
        }
    };

    return (
        <div className="space-y-3">
            <select className="form-input" onChange={e => setStatusId(e.target.value)}>
                <option value="">-- Choose Status --</option>
                <option value="2">In-progress</option>
                <option value="3">Completed</option>
                <option value="4">Rejected</option>
            </select>

            <textarea
                className="form-input"
                placeholder="Optional note"
                onChange={(e) => setNote(e.target.value)}
            />

            <button className="btn-primary" onClick={handleUpdate}>
                Update
            </button>
        </div>
    );
}
