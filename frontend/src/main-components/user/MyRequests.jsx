import { useEffect, useState } from "react";
import enduser from "../../api/enduser";

export default function MyRequests() {
    const [requests, setRequests] = useState(null);

    useEffect(() => {
        enduser.getMyRequests().then((res) => setRequests(res));
    }, []);

    if (requests === null)
        return <div className="text-center text-gray-500">Loading...</div>;

    return (
        <div>
            <h2 className="text-3xl font-bold mb-6 text-gray-800">
                My Requests
            </h2>

            <table className="w-full bg-white rounded-xl shadow overflow-hidden">
                <thead className="bg-gray-50 border-b">
                <tr className="text-left text-gray-700 text-sm">
                    <th className="p-4">ID</th>
                    <th className="p-4">Title</th>
                    <th className="p-4">Severity</th>
                    <th className="p-4">Status</th>
                    <th className="p-4">Created</th>
                    <th className="p-4 text-right">Action</th>
                </tr>
                </thead>
                <tbody>
                {requests.map((r) => (
                    <tr
                        key={r.id}
                        className="border-b hover:bg-gray-50 text-gray-800"
                    >
                        <td className="p-4 font-semibold">#{r.id}</td>
                        <td className="p-4">{r.title}</td>
                        <td className="p-4">{r.severityName}</td>
                        <td className="p-4">{r.statusName}</td>
                        <td className="p-4">{r.createdAt}</td>
                        <td className="p-4 text-right">
                            <a
                                href={`/eu/request/${r.id}`}
                                className="bg-indigo-600 text-white px-3 py-1.5 rounded-lg hover:bg-indigo-700 text-sm"
                            >
                                View
                            </a>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
}
