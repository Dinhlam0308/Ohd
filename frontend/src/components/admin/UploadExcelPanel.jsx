import { useState } from "react";
import admin from "../../api/admin";

export default function UploadExcelPanel() {
    const [file, setFile] = useState(null);
    const [message, setMessage] = useState("");

    const upload = async () => {
        if (!file) return alert("Choose a file");

        try {
            const res = await admin.uploadExcel(file);
            setMessage(`Created ${res.count} users`);
        } catch (err) {
            setMessage("Upload failed");
        }
    };

    return (
        <div className="p-4 bg-white shadow rounded-xl">
            <h2 className="text-lg font-semibold mb-3">Bulk Import Users</h2>

            <input
                type="file"
                accept=".xlsx"
                onChange={(e) => setFile(e.target.files[0])}
            />

            <button
                onClick={upload}
                className="mt-3 px-4 py-2 bg-indigo-600 text-white rounded-lg"
            >
                Upload
            </button>

            {message && <p className="mt-3 text-green-600">{message}</p>}
        </div>
    );
}