import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import technicianApi from "../../api/technician";
import TechUploadImages from "./TechUploadImages";
import TechUpdateStatusModal from "./TechUpdateStatusModal";
export default function TechRequestDetail() {
    const { id } = useParams();
    const [data, setData] = useState(null);

    const load = () => {
        technicianApi.getDetail(id).then(res => setData(res));
    };

    useEffect(load, [id]);

    if (!data) return <div>Loading...</div>;

    return (
        <div>
            <h1 className="text-xl font-bold mb-4">Request #{id}</h1>

            {/* Info */}
            <div className="p-4 bg-white shadow rounded mb-4">
                <p><b>Title:</b> {data.request.title}</p>
                <p><b>Description:</b> {data.request.description}</p>
                <p><b>Status:</b> {data.request.statusId}</p>
            </div>

            {/* Update Status */}
            <TechUpdateStatusModal requestId={id} onUpdated={load} />

            {/* Upload images if status = In Progress */}
            {data.request.statusId === 2 && (
                <TechUploadImages requestId={id} onUploaded={load} />
            )}

            {/* Comments */}
            <h3 className="mt-6 font-bold">Timeline</h3>
            <div className="bg-white p-4 rounded shadow">
                {data.comments.map(c => (
                    <div key={c.id} className="border-b py-2">
                        <p>{c.body}</p>
                        <small className="text-slate-600">
                            {new Date(c.createdAt).toLocaleString()}
                        </small>
                    </div>
                ))}
            </div>
        </div>
    );
}
