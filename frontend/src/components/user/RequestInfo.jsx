export default function RequestInfo({ detail }) {
    if (!detail) return null;

    return (
        <div className="bg-gray-50 p-4 rounded border mb-4">
            <h3 className="text-xl font-semibold mb-3">Request Information</h3>

            <div className="space-y-2">
                <p><strong>ID:</strong> {detail.id}</p>
                <p><strong>Title:</strong> {detail.title}</p>
                <p><strong>Description:</strong> {detail.description || "No description"}</p>

                <p><strong>Facility:</strong> {detail.facilityName}</p>

                <p><strong>Severity:</strong> {detail.severityName}</p>

                <p>
                    <strong>Status:</strong>{" "}
                    <span
                        className="px-2 py-1 text-white rounded"
                        style={{ backgroundColor: detail.statusColor }}
                    >
                        {detail.statusName}
                    </span>
                </p>

                <p><strong>Created At:</strong> {new Date(detail.createdAt).toLocaleString()}</p>

                {detail.dueDate && (
                    <p><strong>Due Date:</strong> {new Date(detail.dueDate).toLocaleString()}</p>
                )}

                {detail.completedAt && (
                    <p><strong>Completed At:</strong> {new Date(detail.completedAt).toLocaleString()}</p>
                )}

                {detail.remarks && (
                    <p><strong>Remarks:</strong> {detail.remarks}</p>
                )}
            </div>
        </div>
    );
}
