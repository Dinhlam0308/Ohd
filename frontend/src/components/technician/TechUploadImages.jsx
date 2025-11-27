import { useState } from "react";
import axios from "axios";

export default function TechUploadImages({ requestId, onUploaded }) {
    const [file, setFile] = useState(null);
    const [loading, setLoading] = useState(false);
    const [preview, setPreview] = useState(null);

    const handleSelect = (e) => {
        const selected = e.target.files[0];
        setFile(selected);

        if (selected) {
            setPreview(URL.createObjectURL(selected));
        }
    };

    const upload = async () => {
        if (!file) {
            alert("Please select an image first.");
            return;
        }

        setLoading(true);

        try {
            const form = new FormData();
            form.append("file", file);
            form.append("upload_preset", "your_preset");

            // Upload to Cloudinary
            const cloud = await axios.post(
                "https://api.cloudinary.com/v1_1/your_cloud/image/upload",
                form
            );

            const imageUrl = cloud.data.secure_url;

            // Send URL to backend
            await axios.post(`/api/technician/requests/${requestId}/attachments`, {
                file: imageUrl
            });

            alert("Image uploaded successfully!");
            setFile(null);
            setPreview(null);
            onUploaded();
        } catch (err) {
            console.error(err);
            alert("Upload failed!");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="bg-white p-4 rounded shadow mb-4">
            <h3 className="font-bold mb-2">Upload Verification Image</h3>

            {/* Preview */}
            {preview && (
                <img
                    src={preview}
                    alt="preview"
                    className="w-40 rounded mb-3 border shadow"
                />
            )}

            {/* File Input */}
            <input type="file" accept="image/*" onChange={handleSelect} />

            <button
                className="btn-primary mt-3"
                onClick={upload}
                disabled={loading}
            >
                {loading ? "Uploading..." : "Upload Image"}
            </button>
        </div>
    );
}
