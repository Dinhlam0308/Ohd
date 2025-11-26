import { useEffect, useState } from "react";
import departmentheadApi from "../../api/departmenthead";
import AssignPopup from "./AssignPopup";

export default function PendingRequests() {
    const [requests, setRequests] = useState([]);
    const [assignId, setAssignId] = useState(null); // popup state

    // Load data
    const load = () => {
        departmentheadApi.getPendingRequests().then((res) => setRequests(res.data));
    };

    useEffect(() => {
        load();
    }, []);

    return (
        <>
            {/* PAGE TITLE */}
            <div className="dh-page-title">⚙️ In Progress</div>
            <div className="dh-page-sub">Requests currently being processed</div>

            {/* TABLE */}
            <div className="dh-table-wrapper">
                <table className="dh-table">
                    <thead>
                    <tr>
                        <th>Title</th>
                        <th>Technician</th>
                        <th>Updated</th>
                        <th>Action</th>
                    </tr>
                    </thead>

                    <tbody>
                    {requests.map((r) => (
                        <tr key={r.id}>
                            <td>{r.title}</td>
                            <td>{r.assigneeName || r.assigneeId || "—"}</td>
                            <td>{new Date(r.updatedAt).toLocaleString()}</td>

                            <td>
                                <button
                                    className="dh-btn-primary"
                                    onClick={() => setAssignId(r.id)}
                                >
                                    Assign
                                </button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>

            {/* POPUP */}
            {assignId && (
                <AssignPopup
                    requestId={assignId}
                    onClose={() => setAssignId(null)}
                    onAssigned={load}
                />
            )}
        </>
    );
}
