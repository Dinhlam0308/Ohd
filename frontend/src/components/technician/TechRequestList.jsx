import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import technicianApi from "../../api/technician";

export default function TechRequestList() {
    const [data, setData] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        technicianApi.getMyRequests().then(res => setData(res.items));
    }, []);

    return (
        <div>
            <h1 className="text-xl font-bold mb-4">My Requests</h1>

            <table className="w-full bg-white shadow rounded overflow-hidden">
                <thead className="bg-slate-200">
                <tr>
                    <th className="p-2 text-left">ID</th>
                    <th className="p-2 text-left">Title</th>
                    <th className="p-2">Severity</th>
                    <th className="p-2">Due</th>
                    <th className="p-2">Status</th>
                    <th className="p-2"></th>
                </tr>
                </thead>

                <tbody>
                {data.map(req => (
                    <tr key={req.id} className="border-b hover:bg-slate-50">
                        <td className="p-2">{req.id}</td>
                        <td className="p-2">{req.title}</td>
                        <td className="p-2 text-center">{req.severityId}</td>
                        <td className="p-2 text-center">{req.dueDate || "—"}</td>
                        <td className="p-2 text-center">{req.statusId}</td>
                        <td className="p-2 text-right">
                            <button
                                className="btn-primary"
                                onClick={() => navigate(`/tech/requests/${req.id}`)}
                            >
                                View
                            </button>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
}
