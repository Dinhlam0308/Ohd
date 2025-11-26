import { useEffect, useState } from "react";
import departmentheadApi from "../../api/departmenthead";

export default function DHDashboard() {
    const [stats, setStats] = useState(null);

    useEffect(() => {
        departmentheadApi.getDashboardStats().then((res) => setStats(res.data));
    }, []);

    return (
        <>
            <div className="dh-page-title">📊 Overview</div>
            <div className="dh-page-sub">Request status in your facility</div>

            <div className="dh-grid">
                <div className="dh-card">
                    <div className="dh-card-label">Total Requests</div>
                    <div className="dh-card-value">{stats?.total ?? 0}</div>
                </div>
                <div className="dh-card">
                    <div className="dh-card-label">New</div>
                    <div className="dh-card-value">{stats?.newReq ?? 0}</div>
                </div>
                <div className="dh-card">
                    <div className="dh-card-label">In Progress</div>
                    <div className="dh-card-value">{stats?.assigned ?? 0}</div>
                </div>
                <div className="dh-card">
                    <div className="dh-card-label">Completed</div>
                    <div className="dh-card-value">{stats?.completed ?? 0}</div>
                </div>
            </div>

            <div className="dh-card-wide">
                <h3 className="dh-card-title">Recent Activity</h3>
                <p className="dh-card-note">(Coming soon)</p>
            </div>
        </>
    );
}
