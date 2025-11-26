// src/routes/RequestDetailsWrapper.jsx
import { Outlet, useParams, useNavigate } from "react-router-dom";


export default function RequestDetailsWrapper() {
    const { id } = useParams();
    const navigate = useNavigate();

    return (
        <div className="rd-shell">
            {/* HEADER */}
            <div className="rd-header">
                <div className="rd-title">
                    📝 Request #{id}
                </div>

                <button
                    className="rd-back-btn"
                    onClick={() => navigate(-1)}
                >
                    ← Back
                </button>
            </div>

            {/* CONTENT */}
            <div className="rd-main">
                <Outlet />
            </div>
        </div>
    );
}
