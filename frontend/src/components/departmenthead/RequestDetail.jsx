// src/components/departmenthead/RequestDetail.jsx
import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import departmentheadApi from "../../api/departmenthead";
import AssignPopup from "./AssignPopup";
import ReassignPopup from "./ReassignPopup";
import CloseRequestPopup from "./CloseRequestPopup";
import "../../assets/css/departmenthead/RequestDetail.css";

export default function RequestDetail() {
    const { id } = useParams();
    const navigate = useNavigate();

    const [request, setRequest] = useState(null);
    const [timeline, setTimeline] = useState([]);
    const [loading, setLoading] = useState(true);

    const [showAssign, setShowAssign] = useState(false);
    const [showReassign, setShowReassign] = useState(false);
    const [showClose, setShowClose] = useState(false);

    const load = async () => {
        try {
            const [detailRes, timelineRes] = await Promise.all([
                departmentheadApi.getRequestDetail(id),
                departmentheadApi.getRequestTimeline(id),
            ]);

            setRequest(detailRes.data);
            setTimeline(timelineRes.data || []);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        load();
    }, [id]);

    if (loading) return <div className="dh-page-sub">Loading...</div>;
    if (!request) return <div className="dh-page-sub">Request not found.</div>;

    return (
        <>
            {/* HEADER */}
            <div className="dh-page-header">
                <button
                    className="dh-back-btn"
                    onClick={() => navigate(-1)}
                >
                    ← Back
                </button>

                <div>
                    <div className="dh-page-title">
                        #{request.id} — {request.title}
                    </div>
                    <div className="dh-page-sub">
                        Status: <strong>{request.statusName || request.statusId}</strong> ·
                        Severity: <strong>{request.severityName || request.severityId}</strong>
                    </div>
                </div>

                <div className="dh-header-actions">
                    <button
                        className="dh-btn-secondary"
                        onClick={() => setShowAssign(true)}
                    >
                        Assign
                    </button>
                    <button
                        className="dh-btn-secondary"
                        onClick={() => setShowReassign(true)}
                    >
                        Reassign
                    </button>
                    <button
                        className="dh-btn-danger"
                        onClick={() => setShowClose(true)}
                    >
                        Close Request
                    </button>
                </div>
            </div>

            {/* TOP GRID */}
            <div className="dh-grid">
                <div className="dh-card">
                    <div className="dh-card-label">Requester</div>
                    <div className="dh-card-value-sm">
                        {request.requesterName || request.requesterEmail || "N/A"}
                    </div>
                    <div className="dh-card-subline">
                        Created at: {new Date(request.createdAt).toLocaleString()}
                    </div>
                </div>
                <div className="dh-card">
                    <div className="dh-card-label">Assigned Technician</div>
                    <div className="dh-card-value-sm">
                        {request.assigneeName || request.assigneeId || "Unassigned"}
                    </div>
                    <div className="dh-card-subline">
                        Last updated: {new Date(request.updatedAt).toLocaleString()}
                    </div>
                </div>
                <div className="dh-card">
                    <div className="dh-card-label">Facility</div>
                    <div className="dh-card-value-sm">
                        {request.facilityName || request.facilityId}
                    </div>
                    <div className="dh-card-subline">
                        Priority: {request.priorityName || request.priorityId || "N/A"}
                    </div>
                </div>
                <div className="dh-card">
                    <div className="dh-card-label">Due date</div>
                    <div className="dh-card-value-sm">
                        {request.dueDate
                            ? new Date(request.dueDate).toLocaleString()
                            : "No due date"}
                    </div>
                    <div className="dh-card-subline">
                        Completed at:{" "}
                        {request.completedAt
                            ? new Date(request.completedAt).toLocaleString()
                            : "Not completed"}
                    </div>
                </div>
            </div>

            {/* BODY */}
            <div className="dh-detail-layout">
                {/* LEFT — DESCRIPTION + ATTACHMENTS */}
                <div className="dh-card dh-detail-main">
                    <h3 className="dh-detail-title">Description</h3>
                    <p className="dh-detail-desc">
                        {request.description || "No description provided."}
                    </p>

                    {/* Placeholder attachments block */}
                    <div className="dh-detail-section">
                        <div className="dh-detail-section-title">Attachments</div>
                        <div className="dh-detail-empty">No attachments UI yet.</div>
                    </div>
                </div>

                {/* RIGHT — TIMELINE */}
                <div className="dh-card dh-detail-side">
                    <h3 className="dh-detail-title">Activity Timeline</h3>
                    <div className="dh-timeline">
                        {timeline.length === 0 && (
                            <div className="dh-detail-empty">No activity yet.</div>
                        )}
                        {timeline.map((item, idx) => (
                            <div key={idx} className="dh-timeline-item">
                                <div className="dot" />
                                <div className="content">
                                    <div className="title">{item.title}</div>
                                    <div className="meta">
                                        {item.userName && <span>{item.userName}</span>}
                                        <span>
                                            {new Date(item.createdAt).toLocaleString()}
                                        </span>
                                    </div>
                                    {item.note && (
                                        <div className="note">{item.note}</div>
                                    )}
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>

            {/* POPUPS */}
            {showAssign && (
                <AssignPopup
                    requestId={request.id}
                    onClose={() => setShowAssign(false)}
                    onAssigned={load}
                />
            )}

            {showReassign && (
                <ReassignPopup
                    requestId={request.id}
                    onClose={() => setShowReassign(false)}
                    onReassigned={load}
                />
            )}

            {showClose && (
                <CloseRequestPopup
                    requestId={request.id}
                    onClose={() => setShowClose(false)}
                    onClosed={() => {
                        load();
                    }}
                />
            )}
        </>
    );
}
