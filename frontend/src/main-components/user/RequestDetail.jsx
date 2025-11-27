import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import enduser from "../../api/enduser";

export default function RequestDetail() {
    const { id } = useParams();
    const [item, setItem] = useState(null);
    const [comment, setComment] = useState("");
    const [closeReason, setCloseReason] = useState("");

    useEffect(() => {
        enduser.getRequestDetail(id).then((res) => setItem(res));
    }, []);

    if (!item)
        return <div className="text-gray-500">Loading...</div>;

    return (
        <div>
            <h2 className="text-3xl font-bold mb-4 text-gray-700">
                Request #{item.id}
            </h2>

            {/* INFO */}
            <div className="bg-white shadow rounded-xl p-6 border mb-6">
                <p><strong>Title:</strong> {item.title}</p>
                <p><strong>Description:</strong> {item.description}</p>
                <p><strong>Facility:</strong> {item.facilityName}</p>
                <p><strong>Severity:</strong> {item.severityName}</p>
                <p><strong>Status:</strong> {item.statusName}</p>
            </div>

            {/* COMMENTS */}
            <div className="bg-white shadow rounded-xl p-6 border mb-6">
                <h3 className="text-xl font-semibold mb-3">Comments</h3>

                <div className="space-y-3">
                    {item.comments.map((c) => (
                        <div className="border p-3 rounded-lg bg-gray-50" key={c.id}>
                            <p className="text-gray-800">{c.body}</p>
                            <p className="text-gray-500 text-sm">At {c.createdAt}</p>
                        </div>
                    ))}
                </div>

                <textarea
                    className="w-full border rounded-lg p-3 mt-4"
                    placeholder="Add a comment…"
                    value={comment}
                    onChange={(e) => setComment(e.target.value)}
                />

                <button
                    className="mt-3 bg-indigo-600 text-white px-4 py-2 rounded-lg hover:bg-indigo-700"
                    onClick={() => {
                        enduser.addComment(id, comment).then(() => {
                            setComment("");
                            enduser.getRequestDetail(id).then((res) => setItem(res));
                        });
                    }}
                >
                    Submit Comment
                </button>
            </div>

            {/* CLOSE REQUEST */}
            <div className="bg-white shadow rounded-xl p-6 border">
                <h3 className="text-xl font-semibold mb-3 text-red-600">
                    Close Request
                </h3>

                <textarea
                    className="w-full border rounded-lg p-3"
                    placeholder="Reason for closing…"
                    value={closeReason}
                    onChange={(e) => setCloseReason(e.target.value)}
                />

                <button
                    className="mt-3 bg-red-600 text-white px-4 py-2 rounded-lg hover:bg-red-700"
                    onClick={() => {
                        enduser.closeRequest(id, closeReason).then(() => {
                            enduser.getRequestDetail(id).then((res) => setItem(res));
                        });
                    }}
                >
                    Close Request
                </button>
            </div>
        </div>
    );
}
