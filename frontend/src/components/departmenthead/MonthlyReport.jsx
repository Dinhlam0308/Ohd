import { useEffect, useState } from "react";
import departmentheadApi from "../../api/departmenthead";

export default function MonthlyReport() {
    const [report, setReport] = useState(null);

    useEffect(() => {
        departmentheadApi.getMonthlyReport().then(res => setReport(res.data));
    }, []);

    if (!report) return <div>Loading...</div>;

    return (
        <>
            <div className="dh-page-title">📈 Monthly Report</div>
            <div className="dh-page-sub">Overview of monthly request performance</div>

            <div className="dh-grid">
                <div className="dh-card">
                    <div className="dh-card-label">Total Requests</div>
                    <div className="dh-card-value">{report.total}</div>
                </div>
                <div className="dh-card">
                    <div className="dh-card-label">Completed</div>
                    <div className="dh-card-value">{report.completed}</div>
                </div>
                <div className="dh-card">
                    <div className="dh-card-label">Completion Rate</div>
                    <div className="dh-card-value">{report.completionRate}%</div>
                </div>
            </div>
        </>
    );
}
