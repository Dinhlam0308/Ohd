import { useEffect, useState } from "react";
import departmentheadApi from "../../api/departmenthead";

export default function NewRequests() {
    const [requests, setRequests] = useState([]);

    useEffect(() => {
        departmentheadApi.getNewRequests().then((res) => setRequests(res.data));
    }, []);

    return (
        <>
            <div className="dh-page-title">📝 New Requests</div>
            <div className="dh-page-sub">Requests that have not been assigned</div>

            <div className="dh-table-wrapper">
                <table className="dh-table">
                    <thead>
                    <tr>
                        <th>Title</th>
                        <th>Severity</th>
                        <th>Created At</th>
                    </tr>
                    </thead>

                    <tbody>
                    {requests.map((r) => (
                        <tr key={r.id}>
                            <td>{r.title}</td>
                            <td><span className="tag-severity">{r.severityId}</span></td>
                            <td>{new Date(r.createdAt).toLocaleString()}</td>
                        </tr>
                    ))}

                    {requests.length === 0 && (
                        <tr>
                            <td colSpan={3} className="dh-empty">No new requests.</td>
                        </tr>
                    )}
                    </tbody>
                </table>
            </div>
        </>
    );
}
