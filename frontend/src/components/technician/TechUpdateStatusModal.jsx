import technicianApi from "../../api/technician";
import { useState } from "react";

export default function TechUpdateStatusModal({ requestId, onUpdated }) {
    const [statusId, setStatusId] = useState(0);
    const [note, setNote] = useState("");

    const submit = async () => {
        await technicianApi.updateStatus(requestId, { statusId, note });
        onUpdated();
        alert("Status updated!");
    };

    return (
        <div className="bg-white p-4 shadow rounded mb-4">
            <h3 className="font-bold mb-2">Update Status</h3>

            <select
                className="form-input mb-3"
                onChange={e => setStatusId(Number(e.target.value))}
            >
                <option value="0">Select...</option>
                <option value="2">In Progress</option>
                <option value="3">Completed</option>
                <option value="4">Rejected</option>
            </select>

            <textarea
                className="form-input mb-3"
                placeholder="Note..."
                value={note}
                onChange={e => setNote(e.target.value)}
            ></textarea>

            <button className="btn-primary" onClick={submit}>
                Save
            </button>
        </div>
    );
}
