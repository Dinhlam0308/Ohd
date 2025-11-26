// src/routes/AdminLayoutWrapper.jsx
import { Outlet, useLocation } from "react-router-dom";
import { useState, useEffect } from "react";
import AdminLayout from "../components/layout/AdminLayout";

export default function AdminLayoutWrapper() {
    const location = useLocation();
    const [activeTab, setActiveTab] = useState("overview");

    useEffect(() => {
        const current = location.pathname.replace("/admin/", "");
        if (["overview", "users", "facilities", "config"].includes(current)) {
            setActiveTab(current);
        }
    }, [location.pathname]);

    return (
        <AdminLayout activeTab={activeTab} setActiveTab={setActiveTab}>
            <Outlet />
        </AdminLayout>
    );
}