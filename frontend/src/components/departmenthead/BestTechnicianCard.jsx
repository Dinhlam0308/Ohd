import { useEffect, useState } from "react";
import departmenthead from "../../api/departmenthead";

export default function BestTechnicianCard({ requestId, onAssign }) {
    const [best, setBest] = useState(null);

    useEffect(() => {
        const load = async () => {
            const res = await departmenthead.getBestTechnicianForRequest(requestId);
            setBest(res.data);
        };
        load();
    }, [requestId]);

    if (!best) return null;

    return (
        <div className="p-4 bg-blue-50 border border-blue-200 rounded-xl shadow-sm mb-4">
            <h3 className="font-bold text-lg mb-2">Suggested Technician</h3>

            <p className="font-semibold">{best.Name}</p>
            <p className="text-sm text-gray-600">Workload: {best.Workload}</p>
            <p className="text-sm text-gray-600">Overdue: {best.Overdue}</p>
            <p className="text-sm text-gray-600">
                Conflict: {best.Conflict ? "❌ Yes" : "✔ No"}
            </p>
            <p className="text-sm font-bold mt-2">Score: {best.Score}</p>

            <button
                className="btn btn-primary mt-3 w-full"
                onClick={() => onAssign(best.TechnicianId)}
            >
                Assign this technician
            </button>
        </div>
    );
}
