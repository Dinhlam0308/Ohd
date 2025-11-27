import { useEffect, useState } from "react";
import technicianApi from "../../api/technician";

export default function TechDashboard() {
    const [stats, setStats] = useState({
        total: 0,
        inProgress: 0,
        completed: 0,
        overdue: 0,
    });
 

    const [items, setItems] = useState([]);
    console.log("Items:", items);
    useEffect(() => {
        technicianApi
            .getMyRequests({ pageSize: 999 })
            .then(res => {
                const list = res.items || [];

                setItems(list);

                setStats({
                    total: list.length,
                    inProgress: list.filter(x => x.StatusId === 2).length,
                    completed: list.filter(x => x.StatusId === 3).length,
                    overdue: list.filter(x => x.IsOverdue).length,
                });
            });
    }, []);

    return (
        <div className="p-6">

            {/* HEADER */}
            <h1 className="text-3xl font-bold mb-6 text-slate-800">
                Technician Dashboard
            </h1>

            {/* TOP STATS */}
            <div className="grid grid-cols-4 gap-4 mb-10">
                <DashboardCard title="Total" value={stats.total} color="blue" />
                <DashboardCard title="In Progress" value={stats.inProgress} color="yellow" />
                <DashboardCard title="Completed" value={stats.completed} color="green" />
                <DashboardCard title="Overdue" value={stats.overdue} color="red" />
            </div>

            {/* LIST */}
            <h2 className="text-2xl font-semibold mb-4 text-slate-800">My Requests</h2>

            <div className="bg-white shadow-lg rounded-xl overflow-hidden">
                <table className="w-full text-left">
                    <thead className="bg-slate-100 text-slate-800">
                    <tr>
                        <th className="p-3">ID</th>
                        <th className="p-3">Title</th>
                        <th className="p-3">Severity</th>
                        <th className="p-3">Due</th>
                        <th className="p-3">Status</th>
                    </tr>
                    </thead>

                    <tbody>
                    {items.length === 0 ? (
                        <tr>
                            <td colSpan="5" className="text-center p-6 text-slate-500">
                                No requests assigned to you.
                            </td>
                        </tr>
                    ) : (
                        items.map(req => (
                            <tr
                                key={req.Id}
                                className="border-b hover:bg-slate-50 transition cursor-pointer"
                            >
                                <td className="p-3 font-medium">{req.Id}</td>

                                <td className="p-3">{req.Title}</td>

                                <td className="p-3">
                                        <span className={`px-3 py-1 text-sm rounded-lg text-white 
                                            ${req.SeverityId === 1 ? "bg-green-500" : ""}
                                            ${req.SeverityId === 2 ? "bg-yellow-500" : ""}
                                            ${req.SeverityId === 3 ? "bg-red-500" : ""}
                                        `}>
                                            {req.SeverityId === 1 && "Low"}
                                            {req.SeverityId === 2 && "Medium"}
                                            {req.SeverityId === 3 && "High"}
                                        </span>
                                </td>

                                <td className="p-3">
                                    {req.DueDate
                                        ? new Date(req.DueDate).toLocaleString()
                                        : "—"}
                                </td>

                                <td className="p-3">
                                        <span
                                            className={`px-3 py-1 text-sm font-medium rounded-lg text-white
                                            ${req.StatusId === 1 ? "bg-gray-500" : ""}
                                            ${req.StatusId === 2 ? "bg-blue-500" : ""}
                                            ${req.StatusId === 3 ? "bg-green-600" : ""}
                                            `}
                                        >
                                            {req.StatusId === 1 && "New"}
                                            {req.StatusId === 2 && "In Progress"}
                                            {req.StatusId === 3 && "Completed"}
                                        </span>
                                </td>
                            </tr>
                        ))
                    )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

// ================== UI CARD ==================
function DashboardCard({ title, value, color }) {
    const colorMap = {
        blue: "bg-blue-100 text-blue-700",
        yellow: "bg-yellow-100 text-yellow-700",
        green: "bg-green-100 text-green-700",
        red: "bg-red-100 text-red-700",
    };

    return (
        <div className={`p-5 rounded-xl shadow-md ${colorMap[color]}`}>
            <div className="text-lg font-semibold">{title}</div>
            <div className="text-3xl font-bold mt-1">{value}</div>
        </div>
    );
}
