// src/components/admin/RecentRequestsTable.jsx
export default function AdminRecentRequests({ requests }) {
    return (
        <div className="admin-panel">
            <div className="admin-panel-header">
                <div>
                    <h2 className="admin-panel-title">Recent Requests</h2>
                    <p className="admin-panel-sub">
                        The 10 most recent requests in the system
                    </p>
                </div>
            </div>

            <div className="overflow-x-auto">
                <table className="admin-table">
                    <thead>
                    <tr>
                        <th>Code</th>
                        <th>Title</th>
                        <th>Requester</th>
                        <th>Facility</th>
                        <th>Status</th>
                        <th>Created At</th>
                    </tr>
                    </thead>

                    <tbody>
                    {(!requests || requests.length === 0) && (
                        <tr>
                            <td
                                colSpan={6}
                                className="text-center py-6 text-slate-400"
                            >
                                No requests yet.
                            </td>
                        </tr>
                    )}

                    {requests &&
                        requests.map((r) => (
                            <tr key={r.id}>
                                <td>{r.code || r.id}</td>
                                <td className="max-w-xs truncate">{r.title}</td>
                                <td>{r.requesterName || r.requester_email}</td>
                                <td>{r.facilityName}</td>
                                <td>{r.statusName}</td>
                                <td>
                                    {r.created_at
                                        ? new Date(r.created_at).toLocaleString()
                                        : ""}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
