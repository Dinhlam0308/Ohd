// src/components/departmenthead/WorkloadDashboard.jsx
import { useEffect, useState } from "react";
import departmentheadApi from "../../api/departmenthead";
import "../../assets/css/departmenthead/Workload.css";

export default function WorkloadDashboard() {
    const [rows, setRows] = useState([]);

    useEffect(() => {
        departmentheadApi.getWorkload().then((res) => setRows(res.data || []));
    }, []);

    return (
        <>
            <div className="dh-page-title">👨‍🔧 Technician Workload</div>
            <div className="dh-page-sub">
                Overview of tickets by technician (New, In progress, Completed, Overdue)
            </div>

            <div className="dh-table-wrapper">
                <table className="dh-table">
                    <thead>
                    <tr>
                        <th>Technician</th>
                        <th>New</th>
                        <th>In Progress</th>
                        <th>Completed</th>
                        <th>Overdue</th>
                    </tr>
                    </thead>
                    <tbody>
                    {rows.map((t) => (
                        <tr key={t.id}>
                            <td>{t.name || t.email || `#${t.id}`}</td>
                            <td>{t.newCount ?? 0}</td>
                            <td>{t.inProgressCount ?? 0}</td>
                            <td>{t.completedCount ?? 0}</td>
                            <td className={t.overdueCount > 0 ? "text-danger" : ""}>
                                {t.overdueCount ?? 0}
                            </td>
                        </tr>
                    ))}
                    {rows.length === 0 && (
                        <tr>
                            <td colSpan={5} className="dh-empty">
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
