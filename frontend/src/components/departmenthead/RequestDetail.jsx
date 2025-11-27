// src/components/departmenthead/RequestDetail.jsx

import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import departmenthead from "../../api/departmenthead";

// POPUPS
import AssignPopup from "./AssignPopup";
import ReassignPopup from "./ReassignPopup";
import CloseRequestPopup from "./CloseRequestPopup";

// COMPONENTS
import BestTechnicianCard from "./BestTechnicianCard";
import AvailableTechniciansModal from "./AvailableTechniciansModal";
import AddCommentBox from "./AddCommentBox";
import TimelineView from "./TimelineView";
import TechnicianStats from "./TechnicianStats";

import "../../assets/css/departmenthead/RequestDetail.css";

export default function RequestDetail() {
    const { id } = useParams(); // <-- requestId từ URL
    const navigate = useNavigate();

    const [request, setRequest] = useState(null);
    const [timeline, setTimeline] = useState([]);
    const [loading, setLoading] = useState(true);

    // POPUPS
    const [showAssign, setShowAssign] = useState(false);
    const [showReassign, setShowReassign] = useState(false);
    const [showClose, setShowClose] = useState(false);
    const [showAvailableModal, setShowAvailableModal] = useState(false);

    // Load data
    const load = async () => {
        try {
            const [detailRes, timelineRes] = await Promise.all([
                departmenthead.getRequestDetail(id),
                departmenthead.getRequestTimeline(id),
            ]);

            setRequest(detailRes.data);
            setTimeline(timelineRes.data || []);
        } catch (err) {
            console.error("Failed to load request:", err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (!id) return;
        load();
    }, [id]);

    if (loading) return <div className="dh-page-sub">Loading...</div>;
    if (!request) return <div className="dh-page-sub">Request not found.</div>;

    return (
        <>
            {/* HEADER */}
            <div className="dh-page-header">
                <button className="dh-back-btn" onClick={() => navigate(-1)}>
                    ← Back
                </button>

                <div>
                    <div className="dh-page-title">
                        #{request.id} — {request.title}
                    </div>
                    <div className="dh-page-sub">
                        Status: <strong>{request.statusName}</strong> ·{" "}
                        Severity: <strong>{request.severityName}</strong>
                    </div>
                </div>

                <div className="dh-header-actions">
                    <button className="dh-btn-secondary" onClick={() => setShowAssign(true)}>
                        Assign
                    </button>
                    <button className="dh-btn-secondary" onClick={() => setShowReassign(true)}>
                        Reassign
                    </button>
                    <button className="dh-btn-secondary" onClick={() => setShowAvailableModal(true)}>
                        Check Availability
                    </button>
                    <button className="dh-btn-danger" onClick={() => setShowClose(true)}>
                        Close Request
                    </button>
                </div>
            </div>

            {/* 🔥 BEST TECHNICIAN SUGGESTION */}
            <BestTechnicianCard
                requestId={id}
                onAssign={async (techId) => {
                    await departmenthead.assignRequest({
                        requestId: id,
                        assigneeId: techId
                    });
                    load();
                }}
            />

            {/* GRID INFO */}
            <div className="dh-grid">
                <div className="dh-card">
                    <div className="dh-card-label">Requester</div>
                    <div className="dh-card-value-sm">
                        {request.requesterName || "N/A"}
                    </div>
                    <div className="dh-card-subline">
                        Created: {new Date(request.createdAt).toLocaleString()}
                    </div>
                </div>

                <div className="dh-card">
                    <div className="dh-card-label">Technician</div>
                    <div className="dh-card-value-sm">
                        {request.assigneeName || "Unassigned"}
                    </div>
                    <div className="dh-card-subline">
                        Updated: {new Date(request.updatedAt).toLocaleString()}
                    </div>

                    {request.assigneeId && (
                        <TechnicianStats technicianId={request.assigneeId} />
                    )}
                </div>

                <div className="dh-card">
                    <div className="dh-card-label">Facility</div>
                    <div className="dh-card-value-sm">
                        {request.facilityName}
                    </div>
                    <div className="dh-card-subline">
                        Priority: {request.priorityName}
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
                        Completed:{" "}
                        {request.completedAt
                            ? new Date(request.completedAt).toLocaleString()
                            : "Not completed"}
                    </div>
                </div>
            </div>

            {/* BODY LAYOUT */}
            <div className="dh-detail-layout">
                {/* LEFT MAIN: DESCRIPTION */}
                <div className="dh-card dh-detail-main">
                    <h3 className="dh-detail-title">Description</h3>
                    <p className="dh-detail-desc">
                        {request.description || "No description provided."}
                    </p>

                    {/* Comments */}
                    <AddCommentBox requestId={id} onAdded={load} />
                </div>

                {/* RIGHT COLUMN: TIMELINE */}
                <div className="dh-card dh-detail-side">
                    <TimelineView requestId={id} />
                </div>
            </div>

            {/* POPUPS */}
            {showAssign && (
                <AssignPopup
                    requestId={id}
                    onClose={() => setShowAssign(false)}
                    onAssigned={load}
                />
            )}

            {showReassign && (
                <ReassignPopup
                    requestId={id}
                    onClose={() => setShowReassign(false)}
                    onReassigned={load}
                />
            )}

            {showClose && (
                <CloseRequestPopup
                    requestId={id}
                    onClose={() => setShowClose(false)}
                    onClosed={load}
                />
            )}

            {showAvailableModal && (
                <AvailableTechniciansModal
                    requestId={id}
                    onSelect={async (techId) => {
                        await departmenthead.assignRequest({
                            requestId: id,
                            assigneeId: techId
                        });
                        setShowAvailableModal(false);
                        load();
                    }}
                    onClose={() => setShowAvailableModal(false)}
                />
            )}
        </>
    );
}
