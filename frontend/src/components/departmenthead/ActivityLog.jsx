// src/components/departmenthead/ActivityLog.jsx
import { useEffect, useMemo, useState } from "react";
import departmentheadApi from "../../api/departmenthead";
import FilterPanel from "./FiltersPanel";
import "../../assets/css/departmenthead/ActivityLog.css";
export default function ActivityLog() {
    const [items, setItems] = useState([]);
    const [filters, setFilters] = useState({
        search: "",
        from: "",
        to: "",
        status: "",
    });
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const load = async () => {
            setLoading(true);
            try {
                const res = await departmentheadApi.getActivityLog();
                setItems(res.data || []);
            } catch (err) {
                console.error("Load activity log failed:", err);
            } finally {
                setLoading(false);
            }
        };
        load();
    }, []);

    const filteredItems = useMemo(() => {
        return (items || []).filter((item) => {
            const created =
                item.createdAt || item.created_at
                    ? new Date(item.createdAt || item.created_at)
                    : null;

            // Search filter
            if (filters.search) {
                const s = filters.search.toLowerCase();
                const text = (
                    (item.userName || item.user_name || "") +
                    " " +
                    (item.action || item.title || "") +
                    " " +
                    (item.requestId || item.request_id || "")
                ).toLowerCase();
                if (!text.includes(s)) return false;
            }

            // From date
            if (filters.from && created) {
                const fromDate = new Date(filters.from);
                if (created < fromDate) return false;
            }

            // To date
            if (filters.to && created) {
                const toDate = new Date(filters.to);
                // +1 ngày để inclusive
                toDate.setDate(toDate.getDate() + 1);
                if (created >= toDate) return false;
            }

            // Status filter (optional – tùy bạn map)
            if (filters.status) {
                const statusId = item.statusId || item.status_id;
                const isOverdue = item.isOverdue || false;

                switch (filters.status) {
                    case "new":
                        if (statusId !== 1) return false;
                        break;
                    case "assigned":
                        if (statusId !== 2) return false;
                        break;
                    case "completed":
                        if (statusId !== 4) return false;
                        break;
                    case "overdue":
                        if (!isOverdue) return false;
                        break;
                    default:
                        break;
                }
            }

            return true;
        });
    }, [items, filters]);

    return (
        <>
            <div className="dh-page-title">📜 Activity Log</div>
            <div className="dh-page-sub">
                All request activities in your facility (status updates, assignments, comments, etc.).
            </div>

            {/* FILTER */}
            <FilterPanel filters={filters} onChange={setFilters} />

            {/* LIST */}
            <div className="dh-activity-container">
                {loading && <div className="dh-page-sub">Loading...</div>}

                {!loading && filteredItems.length === 0 && (
                    <div className="dh-detail-empty">
                        No activity matches current filters.
                    </div>
                )}

                {!loading &&
                    filteredItems.map((a, idx) => (
                        <div key={idx} className="dh-activity-item">
                            <div className="dh-activity-time">
                                {a.createdAt || a.created_at
                                    ? new Date(a.createdAt || a.created_at).toLocaleString()
                                    : "-"}
                            </div>

                            <div className="dh-activity-main">
                                <div className="dh-activity-line">
                                    <strong>{a.userName || a.user_name || "System"}</strong>{" "}
                                    <span className="action-text">
                                        {a.action || a.title || "did something"}
                                    </span>
                                </div>

                                <div className="dh-activity-meta">
                                    <span>Request #{a.requestId || a.request_id}</span>
                                    {a.statusName && <span> · {a.statusName}</span>}
                                    {a.statusId && !a.statusName && (
                                        <span> · Status #{a.statusId}</span>
                                    )}
                                </div>

                                {a.note && (
                                    <div className="dh-activity-note">
                                        {a.note}
                                    </div>
                                )}
                            </div>
                        </div>
                    ))}
            </div>
        </>
    );
}
