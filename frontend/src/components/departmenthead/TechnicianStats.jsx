import { useEffect, useState } from "react";
import departmenthead from "../../api/departmenthead";

export default function TechnicianStats({ technicianId }) {
    const [stats, setStats] = useState(null);

    useEffect(() => {
        const load = async () => {
            const res = await departmenthead.getTechnicianStats(technicianId);
            setStats(res.data);
        };
        load();
    }, [technicianId]);

    if (!stats) return null;

    return (
        <div className="bg-white border p-4 rounded-xl mt-3">
            <h3 className="font-bold">Technician KPI</h3>
            <p className="text-gray-600">Completed: {stats.completed}</p>
            <p className="text-gray-600">Assigned: {stats.assigned}</p>
            <p className="text-gray-600">Overdue: {stats.overdue}</p>
        </div>
    );
}
