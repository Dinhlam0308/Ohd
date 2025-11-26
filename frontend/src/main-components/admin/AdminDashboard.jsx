// src/main-components/admin/AdminDashboard.jsx
import { useEffect, useState } from "react";
import adminApi from "../../api/admin";
import requestApi from "../../api/request"; // ⭐ thêm API request ở đây

import AdminStatCards from "../../components/admin/DashboardStats.jsx";
import AdminRecentRequests from "../../components/admin/RecentRequestsTable.jsx";
import OverdueChart from "../../components/admin/OverdueChart.jsx";

export default function AdminDashboard() {
    const [stats, setStats] = useState(null);
    const [latestRequests, setLatestRequests] = useState([]);
    const [overdueCount, setOverdueCount] = useState(0); // ⭐ thêm state
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const loadData = async () => {
        try {
            setLoading(true);
            setError("");

            const [dashboard, requests, overdue] = await Promise.all([
                adminApi.getDashboard(),
                adminApi.getRequests({ limit: 10 }),
                requestApi.getOverdueCount(), // ⭐ gọi API số lượng overdue
            ]);

            setStats({
                ...dashboard,
                overdueCount: overdue.overdue, // ⭐ thêm số lượng vào stats
            });

            setLatestRequests(requests);

        } catch (err) {
            console.error(err);
            setError("Không thể tải dữ liệu dashboard.");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadData();
    }, []);

    if (loading) {
        return (
            <div className="admin-loading">
                <div className="spinner" />
                <p>Đang tải dữ liệu...</p>
            </div>
        );
    }

    if (error) {
        return (
            <div className="admin-error">
                <p>{error}</p>
                <button onClick={loadData}>Thử lại</button>
            </div>
        );
    }

    return (
        <div className="space-y-6">
            {/* ⭐ PASS THÊM overdueCount xuống StatCards */}
            <AdminStatCards stats={stats} />
            <OverdueChart />
            <AdminRecentRequests requests={latestRequests} />
        </div>
    );
    
}
