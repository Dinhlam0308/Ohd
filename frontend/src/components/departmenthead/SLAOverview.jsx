// src/components/departmenthead/SLAOverview.jsx
import { useEffect, useState } from "react";
import departmentheadApi from "../../api/departmenthead";
import "../../assets/css/departmenthead/SLA.css";

export default function SLAOverview() {
    const [data, setData] = useState(null);

    useEffect(() => {
        departmentheadApi.getSLAOverview().then((res) => setData(res.data));
    }, []);

    if (!data) return <div className="dh-page-sub">Loading...</div>;

    return (
        <>
            <div className="dh-page-title">⏱ SLA Overview</div>
            <div className="dh-page-sub">
                Monitor on-time vs overdue tickets and SLA compliance.
            </div>

            <div className="dh-grid">
                <div className="dh-card">
                    <div className="dh-card-label">Total tickets</div>
                    <div className="dh-card-value">{data.total ?? 0}</div>
                </div>
                <div className="dh-card">
                    <div className="dh-card-label">On-time</div>
                    <div className="dh-card-value">{data.onTime ?? 0}</div>
                </div>
                <div className="dh-card">
                    <div className="dh-card-label">Overdue</div>
                    <div className="dh-card-value">{data.overdue ?? 0}</div>
                </div>
                <div className="dh-card">
                    <div className="dh-card-label">SLA Compliance</div>
                    <div className="dh-card-value">
                        {data.complianceRate ?? 0}%
                    </div>
                </div>
            </div>

            <div className="dh-card-wide">
                <h3 className="dh-card-title">Notes</h3>
                <p className="dh-card-note">
                    You can later replace this section with charts (e.g., React Charts)
                    showing SLA by category or severity.
                </p>
            </div>
        </>
    );
}
