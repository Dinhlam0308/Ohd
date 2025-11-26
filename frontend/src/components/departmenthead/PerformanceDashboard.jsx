// src/components/departmenthead/PerformanceDashboard.jsx
import { useEffect, useState } from "react";
import departmentheadApi from "../../api/departmenthead";
import "../../assets/css/departmenthead/Performance.css";

export default function PerformanceDashboard() {
    const [rows, setRows] = useState([]);

    useEffect(() => {
        departmentheadApi.getPerformance().then((res) => setRows(res.data || []));
    }, []);

    return (
        <>
            <div className="dh-page-title">📈 Technician Performance</div>
            <div className="dh-page-sub">
                Monthly KPI: closed tickets, overdue issues, resolution time.
            </div>

            <div className="dh-table-wrapper">
                <table className="dh-table">
                    <thead>
                    <tr>
                        <th>Technician</th>
                        <th>Completed</th>
                        <th>Overdue</th>
                        <th>Avg Resolution (hrs)</th>
                    </tr>
                    </thead>

                    <tbody>
                    {rows.map((t) => (
                        <tr key={t.id}>
                            <td>
                                {t.username}
                            </td>
                            <td>{t.completed ?? 0}</td>
                            <td>{t.overdue ?? 0}</td>
                            <td>{t.avgResolutionHours ?? "-"}</td>
                        </tr>
                    ))}

                    {rows.length === 0 && (
                        <tr>
                            <td colSpan={4} className="dh-empty">
                                No data yet.
                            </td>
                        </tr>
                    )}
                    </tbody>
                </table>
            </div>
        </>
    );
}
