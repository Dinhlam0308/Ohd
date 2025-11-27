import { useEffect, useState } from "react";
import enduser from "../../api/enduser";
import lookup from "../../api/lookup";

export default function CreateRequest() {
    const [form, setForm] = useState({
        title: "",
        description: "",
        facilityId: "",
        severityId: "",
    });

    const [facilities, setFacilities] = useState([]);
    const [severities, setSeverities] = useState([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        loadLookups();
    }, []);

    const loadLookups = async () => {
        try {
            const fs = await lookup.getFacilities();
            const sv = await lookup.getSeverities();

            console.log("Facilities:", fs);
            console.log("Severities:", sv);

            setFacilities(Array.isArray(fs) ? fs : []);
            setSeverities(Array.isArray(sv) ? sv : []);
        } catch (err) {
            console.error("Lookup error:", err);
        }
    };

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async () => {
        if (!form.title.trim()) return alert("Title is required.");
        if (!form.facilityId) return alert("Please select a facility.");
        if (!form.severityId) return alert("Please select a severity.");

        setLoading(true);
        try {
            await enduser.createRequest(form);
            alert("Request created successfully.");

            setForm({
                title: "",
                description: "",
                facilityId: "",
                severityId: "",
            });
        } catch (err) {
            console.error(err);
            alert("Failed to create request.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="max-w-2xl mx-auto text-black w-[100vw]">
            <h2 className="text-3xl font-bold mb-6">Create Request</h2>

            <div className="bg-white shadow rounded-xl p-6 border space-y-4">

                {/* TITLE */}
                <div>
                    <label className="font-medium">Title</label>
                    <input
                        name="title"
                        value={form.title}
                        placeholder="Request title..."
                        className="w-full border rounded-lg p-3 mt-1"
                        onChange={handleChange}
                    />
                </div>

                {/* DESCRIPTION */}
                <div>
                    <label className="font-medium">Description</label>
                    <textarea
                        name="description"
                        value={form.description}
                        placeholder="Describe the issue..."
                        className="w-full border rounded-lg p-3 mt-1 min-h-[120px]"
                        onChange={handleChange}
                    />
                </div>

                {/* FACILITY */}
                <div>
                    <label className="font-medium">Facility</label>
                    <select
                        name="facilityId"
                        value={form.facilityId}
                        className="w-full border rounded-lg p-3 mt-1"
                        onChange={handleChange}
                    >
                        <option value="">-- Select facility --</option>
                        {facilities.map((f) => (
                            <option key={f.id} value={f.id}>
                                {f.name}
                            </option>
                        ))}
                    </select>
                </div>

                {/* SEVERITY */}
                <div>
                    <label className="font-medium">Severity</label>
                    <select
                        name="severityId"
                        value={form.severityId}
                        className="w-full border rounded-lg p-3 mt-1"
                        onChange={handleChange}
                    >
                        <option value="">-- Select severity --</option>
                        {severities.map((s) => (
                            <option key={s.id} value={s.id}>
                                {s.name}
                            </option>
                        ))}
                    </select>
                </div>

                {/* SUBMIT BUTTON */}
                <button
                    onClick={handleSubmit}
                    disabled={loading}
                    className="bg-indigo-600 text-white px-5 py-2.5 rounded-lg hover:bg-indigo-700 disabled:opacity-50"
                >
                    {loading ? "Submitting..." : "Submit"}
                </button>
            </div>
        </div>
    );
}
