// src/components/admin/OverdueTimelineAnalysis.jsx
import { useEffect, useState } from "react";
import requestApi from "../../api/request";

export default function OverdueTimelineAnalysis() {
    const [requests, setRequests] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const load = async () => {
            try {
                const res = await requestApi.getFrontend(); // dùng API mới
                setRequests(res.data);
            } catch (err) {
                console.error("Load requests error:", err);
            } finally {
                setLoading(false);
            }
        };

        load();
    }, []);

    if (loading) {
        return (
            <div className="admin-panel">
                <div className="p-6 text-slate-300 text-center">Loading...</div>
            </div>
        );
    }

    if (!requests.length) {
        return (
            <div className="admin-panel">
                <div className="p-6 text-slate-400 text-center">No requests found.</div>
            </div>
        );
    }

    // === Filter overdue items ===
    const overdueItems = requests.filter((r) => r.isOverdue);

    // === Group by DueDate (YYYY-MM-DD) ===
    const groups = overdueItems.reduce((acc, r) => {
        const key = r.dueDate ? r.dueDate.slice(0, 10) : "Unknown";
        if (!acc[key]) acc[key] = [];
        acc[key].push(r);
        return acc;
    }, {});

    const sortedDates = Object.keys(groups).sort();

    return (
        <div className="admin-panel">
            <div className="admin-panel-header">
                <h2 className="admin-panel-title">Overdue Request Timeline</h2>
                <p className="admin-panel-sub text-slate-300">
                    Analysis of all overdue deadlines by date
                </p>
            </div>

            {overdueItems.length === 0 ? (
                <p className="text-center text-slate-400 py-6">
                    No overdue requests — excellent performance!
                </p>
            ) : (
                <div className="space-y-5">
                    {sortedDates.map((date) => (
                        <div
                            key={date}
                            className="rounded-xl border border-red-500/30 p-4 bg-red-900/20 backdrop-blur-md shadow-md"
                        >
                            {/* DATE HEADER */}
                            <div className="text-red-300 font-semibold mb-3 flex items-center gap-2">
                                <span className="text-xl">📅</span> {date}
                            </div>

                            {/* REQUEST ITEMS */}
                            {groups[date].map((req) => (
                                <div
                                    key={req.id}
                                    className="bg-red-500/10 rounded-lg border border-red-400/20 p-4 mb-3 hover:bg-red-500/20 transition"
                                >
                                    <div className="font-semibold text-white text-lg">
                                        {req.title}
                                    </div>

                                    {/* Deadline */}
                                    <div className="text-sm text-red-200 mt-1">
                                        Deadline:{" "}
                                        <span className="font-medium text-red-300">
                                            {new Date(req.dueDate).toLocaleString()}
                                        </span>
                                    </div>

                                    {/* Completion */}
                                    <div className="text-sm text-red-200">
                                        Completed:{" "}
                                        <span className="font-medium text-red-300">
                                            {req.completedAt
                                                ? new Date(req.completedAt).toLocaleString()
                                                : "Not completed"}
                                        </span>
                                    </div>

                                    {/* Status Tag */}
                                    <div className="mt-2 inline-block px-3 py-1 rounded-full text-xs bg-red-800/40 text-red-200 border border-red-500/30">
                                        ⚠ Overdue
                                    </div>
                                </div>
                            ))}
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}
