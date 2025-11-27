import { useEffect, useState } from "react";
import departmenthead from "../../api/departmenthead";

export default function TimelineView({ requestId }) {
    const [timeline, setTimeline] = useState([]);

    useEffect(() => {
        const load = async () => {
            const res = await departmenthead.getRequestTimeline(requestId);
            setTimeline(res.data);
        };
        load();
    }, [requestId]);

    return (
        <div className="p-4 bg-white rounded-xl border mt-4">
            <h3 className="font-bold text-lg mb-3">Timeline</h3>

            {timeline.length === 0 ? (
                <p className="text-gray-500">No activity yet.</p>
            ) : (
                <ul className="space-y-3">
                    {timeline.map((item) => (
                        <li key={item.id} className="p-3 border rounded-lg">
                            <p className="font-semibold">{item.title}</p>
                            <p className="text-gray-600 text-sm">{item.note}</p>
                            <p className="text-xs mt-1 text-gray-400">
                                {new Date(item.created_at).toLocaleString()} — {item.user_name}
                            </p>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}
