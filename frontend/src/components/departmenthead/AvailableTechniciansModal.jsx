import { useState, useEffect } from "react";
import departmenthead from "../../api/departmenthead";

export default function AvailableTechniciansModal({ requestId, onSelect, onClose }) {
    const [dateTime, setDateTime] = useState("");
    const [available, setAvailable] = useState([]);

    const loadAvailable = async () => {
        if (!dateTime) return;
        const res = await departmenthead.getAvailableTechnicians(requestId, dateTime);
        setAvailable(res.data);
    };

    return (
        <div className="fixed inset-0 bg-black/40 flex items-center justify-center p-4">
            <div className="bg-white rounded-xl shadow-xl w-full max-w-lg p-5">
                <h2 className="text-xl font-semibold mb-4">Technicians Available</h2>

                <input
                    type="datetime-local"
                    className="input input-bordered w-full mb-3"
                    value={dateTime}
                    onChange={(e) => setDateTime(e.target.value)}
                />

                <button onClick={loadAvailable} className="btn btn-primary mb-4 w-full">
                    Check available technicians
                </button>

                {available.length === 0 ? (
                    <p className="text-gray-500">No technicians available.</p>
                ) : (
                    <ul className="divide-y">
                        {available.map((t) => (
                            <li key={t.Id} className="py-2 flex justify-between items-center">
                                <div>
                                    <p className="font-medium">{t.Username}</p>
                                    <p className="text-sm text-gray-600">{t.Email}</p>
                                </div>
                                <button
                                    className="btn btn-sm btn-success"
                                    onClick={() => onSelect(t.Id)}
                                >
                                    Assign
                                </button>
                            </li>
                        ))}
                    </ul>
                )}

                <button className="btn btn-outline mt-4 w-full" onClick={onClose}>Close</button>
            </div>
        </div>
    );
}
