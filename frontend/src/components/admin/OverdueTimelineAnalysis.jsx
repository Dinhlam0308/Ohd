// src/components/admin/OverdueTimelineAnalysis.jsx
import { useEffect, useState } from "react";
import requestApi from "../../api/request"; // <-- API bạn gửi ở cuối

export default function OverdueTimelineAnalysis() {
    const [requests, setRequests] = useState([]);
    const [loading, setLoading] = useState(true);

    // === Fetch real data from API ===
    useEffect(() => {
        const load = async () => {
            try {
                const res = await requestApi.getAll();
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

    // ---- Calculate overdue entries ----
    const overdueItems = requests.filter((r) => {
        if (!r.dueDate) return false; // backend trả camelCase hay snakeCase tùy bạn

        const due = new Date(r.dueDate);
        const done = r.completedAt ? new Date(r.completedAt) : null;
        const now = new Date();

        if (done && done > due) return true;         // completed late
        if (!done && now > due) return true;         // still open & overdue

        return false;
    });

    // ---- Group overdue by date (YYYY-MM-DD) ----
    const groups = {};
    overdueItems.forEach((r) => {
        const key = r.dueDate.slice(0, 10);
        if (!groups[key]) groups[key] = [];
        groups[key].push(r);
    });

    const sortedDates = Object.keys(groups).sort();

    return (
        <div className="admin-panel">
            <div className="admin-panel-header">
                <h2 className="admin-panel-title">Overdue Request Timeline</h2>
                <p className="admin-panel-sub">
                    Analysis of all overdue deadlines by date
                </p>
            </div>

            {overdueItems.length === 0 ? (
                <p className="text-center text-slate-400 py-6">
                    No overdue requests — excellent performance!
                </p>
            ) : (
                <div className="space-y-4">
                    {sortedDates.map((date) => (
                        <div
                            key={date}
                            className="rounded-xl border border-red-500/30 p-4 bg-red-900/20 backdrop-blur"
                        >
                            <div className="text-red-300 font-semibold mb-3">
                                📅 {date}
                            </div>

                            {groups[date].map((req) => (
                                <div
                                    key={req.id}
                                    className="bg-red-500/10 p-3 rounded-lg border border-red-400/20 mb-2"
                                >
                                    <div className="font-medium text-white">{req.title}</div>
                                    <div className="text-sm text-red-200">
                                        Deadline: {req.dueDate}
                                    </div>
                                    <div className="text-sm text-red-200">
                                        Completed:{" "}
                                        {req.completedAt || "Not completed"}
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
