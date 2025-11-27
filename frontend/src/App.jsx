import { Routes, Route, Navigate } from "react-router-dom";
import { useEffect, useState } from "react";

// Public pages
import LoginPage from "./components/login/Login.jsx";
import ForgotPassword from "./components/forgotpassword/ForgotPassword.jsx";
import ChangePasswordFirstLoginPage from "./components/changepassword/ChangePasswordFirstLogin.jsx";

// Admin pages
import AdminDashboard from "./main-components/admin/AdminDashboard.jsx";
import AdminConfigPanel from "./components/admin/AdminConfigPanel.jsx";
import AdminFacilities from "./components/admin/AdminFacilitiesPanel.jsx";
import AdminUserPanel from "./components/admin/AdminUsersPanel.jsx";

// Department Head pages
import NewRequests from "./components/departmenthead/NewRequests.jsx";
import PendingRequests from "./components/departmenthead/PendingRequests.jsx";
import Notifications from "./components/departmenthead/Notifications.jsx";
import Dashboard from "./main-components/departmenthead/Dashboard.jsx";
import MonthlyReport from "./components/departmenthead/MonthlyReport.jsx";
import DepartmentHeadLayoutWrapper from "./routes/DepartmentHeadLayoutWrapper";
import RequestDetail from "./components/departmenthead/RequestDetail.jsx";
import WorkloadDashboard from "./components/departmenthead/WorkloadDashboard.jsx";
import SLAOverview from "./components/departmenthead/SLAOverview.jsx";
import ActivityLog from "./components/departmenthead/ActivityLog.jsx";
import PerformanceDashboard from "./components/departmenthead/PerformanceDashboard.jsx";
import BulkAssign from "./components/departmenthead/BulkAssign.jsx";
import BestTechnicianPage  from "./components/departmenthead/BestTechnicianCard.jsx";
import AvailableTechniciansPage from "./components/departmenthead/AvailableTechniciansModal.jsx";


import TechnicianLayoutWrapper from "./routes/TechnicianLayoutWrapper";
import TechDashboard from "./main-components/technician/TechDashboard";
import TechRequestList from "./components/technician/TechRequestList";
import TechRequestDetail from "./components/technician/TechRequestDetail";
// END-USER
import EndUserLayoutWrapper from "./routes/EndUserLayoutWrapper.jsx";
import CreateRequest from "./main-components/user/CreateRequest.jsx";
import MyRequests from "./main-components/user/MyRequests.jsx";
import EndUserRequestDetail from "./main-components/user/RequestDetail.jsx";


import AdminLayoutWrapper from "./routes/AdminLayoutWrapper.jsx";
import ProtectedRoute from "./routes/ProtectedRoute.jsx";
import RequestDetailWrapper from "./routes/RequestDetailWrapper.jsx";
export default function App() {
    const [authReady, setAuthReady] = useState(false);
    const [token, setToken] = useState(null);

    useEffect(() => {
        const tk = localStorage.getItem("token");
        setToken(tk);
        setAuthReady(true);
    }, []);

    if (!authReady) return <div>Loading...</div>;

    return (
        <Routes>

            {/* Redirect 5173/ → login */}
            <Route path="/" element={<Navigate to="/login" replace />} />

            {/* PUBLIC ROUTES */}
            <Route path="/login" element={<LoginPage />} />
            <Route path="/forgot-password" element={<ForgotPassword />} />
            <Route path="/change-password-first-login" element={<ChangePasswordFirstLoginPage />} />


            {/* ====================== ADMIN ROUTES ====================== */}
            <Route
                path="/admin"
                element={
                    <ProtectedRoute allowedRoles={["Admin"]}>
                        <AdminLayoutWrapper />
                    </ProtectedRoute>
                }
            >
                <Route index element={<Navigate to="/admin/dashboard" replace />} />

                <Route path="dashboard" element={<AdminDashboard />} />
                <Route path="config" element={<AdminConfigPanel />} />
                <Route path="facilities" element={<AdminFacilities />} />
                <Route path="users" element={<AdminUserPanel />} />
            </Route>


            {/* ================== DEPARTMENT HEAD ROUTES ================= */}
            <Route path="/dh" element={
                <ProtectedRoute allowedRoles={["DepartmentHead"]}>
                    <DepartmentHeadLayoutWrapper />
                </ProtectedRoute>
            }>
                <Route index element={<Navigate to="/dh/dashboard" replace />} />

                <Route path="dashboard" element={<Dashboard />} />
                <Route path="requests/new" element={<NewRequests />} />
                <Route path="requests/pending" element={<PendingRequests />} />
                <Route path="notifications" element={<Notifications />} />
                <Route path="report" element={<MonthlyReport />} />

                {/* NEW advanced features */}
                <Route path="workload" element={<WorkloadDashboard />} />
                <Route path="sla" element={<SLAOverview />} />
                <Route path="activity" element={<ActivityLog />} />
                <Route path="performance" element={<PerformanceDashboard />} />
                <Route path="bulk-assign" element={<BulkAssign />} />

                {/* Request Detail */}
                <Route path="technicians/best" element={<BestTechnicianPage />} />
                <Route path="technicians/available" element={<AvailableTechniciansPage />} />
                <Route path="request/:id" element={<RequestDetail />} />
            </Route>


            {/* TECHNICIAN */}
            <Route
                path="/tech"
                element={
                    <ProtectedRoute allowedRoles={["Technician"]}>
                        <TechnicianLayoutWrapper />
                    </ProtectedRoute>
                }
            >
                <Route index element={<Navigate to="/tech/dashboard" replace />} />
                <Route path="dashboard" element={<TechDashboard />} />
                <Route path="requests" element={<TechRequestList />} />
                <Route path="request/:id" element={<TechRequestDetail />} />
            </Route>
            <Route
                path="/eu"
                element={
                    <ProtectedRoute>
                        <EndUserLayoutWrapper />
                    </ProtectedRoute>
                }
            >
                <Route index element={<Navigate to="" replace />} />

                <Route path="requests" element={<MyRequests />} />
                <Route path="requests/create" element={<CreateRequest />} />
                <Route path="request/:id" element={<EndUserRequestDetail />} />
            </Route>

            {/* DEFAULT CATCH-ALL */}
            <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
    );
}
