import { useEffect, useState } from "react";
import technicianApi from "../../api/technician";

export default function TechDashboard() {
    const [stats, setStats] = useState(null);

    useEffect(() => {
        technicianApi.getMyRequests({ pageSize: 999 }).then(res => {
            const items = res.items;

            setStats({
                total: items.length,
                inProgress: items.filter(r => r.statusId === 2).length,
                completed: items.filter(r => r.statusId === 3).length,
                overdue: items.filter(r => r.isOverdue).length,
            });
        });
    }, []);

    if (!stats) return <div>Loading...</div>;

    return (
        <div>
            <h1 className="text-2xl font-bold mb-6">Technician Dashboard</h1>

            <div className="grid md:grid-cols-4 gap-4">
                <div className="p-4 bg-white shadow rounded">Total: {stats.total}</div>
                <div className="p-4 bg-white shadow rounded">In Progress: {stats.inProgress}</div>
                <div className="p-4 bg-white shadow rounded">Completed: {stats.completed}</div>
                <div className="p-4 bg-white shadow rounded">Overdue: {stats.overdue}</div>
            </div>
        </div>
    );
}
