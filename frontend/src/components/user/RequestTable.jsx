import StatusBadge from "./StatusBadge";

export default function RequestTable({ requests }) {
    if (!requests || requests.length === 0)
        return <div className="p-4 text-gray-500">No requests found.</div>;

    return (
        <table className="w-full border-collapse">
            <thead>
            <tr className="bg-gray-100">
                <th className="p-2 border">ID</th>
                <th className="p-2 border">Title</th>
                <th className="p-2 border">Facility</th>
                <th className="p-2 border">Severity</th>
                <th className="p-2 border">Status</th>
                <th className="p-2 border">Created</th>
            </tr>
            </thead>

            <tbody>
            {requests.map((r) => (
                <tr key={r.id} className="border hover:bg-gray-50 cursor-pointer">
                    <td className="p-2">{r.id}</td>
                    <td className="p-2">{r.title}</td>
                    <td className="p-2">{r.facilityName}</td>
                    <td className="p-2">{r.severityName}</td>
                    <td className="p-2">
                        <StatusBadge name={r.statusName} color={r.statusColor} />
                    </td>
                    <td className="p-2">{new Date(r.createdAt).toLocaleString()}</td>
                </tr>
            ))}
            </tbody>
        </table>
    );
}
