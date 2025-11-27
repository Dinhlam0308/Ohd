// src/routes/DepartmentHeadLayoutWrapper.jsx
import { Outlet, useNavigate, useLocation } from "react-router-dom";
import "../assets/css/departmenthead/DepartmentHead.css";

export default function DepartmentHeadLayoutWrapper() {
    const navigate = useNavigate();
    const location = useLocation();

    const menuItems = [
        { key: "dashboard", label: "Overview", icon: "📊", path: "/dh/dashboard" },
        { key: "new", label: "New Requests", icon: "📝", path: "/dh/requests/new" },
        { key: "pending", label: "In Progress", icon: "⚙️", path: "/dh/requests/pending" },
        { key: "notifications", label: "Notifications", icon: "🔔", path: "/dh/notifications" },
        { key: "report", label: "Monthly Report", icon: "📈", path: "/dh/report" },
        { key: "activity", label: "Activity Log", icon: "📜", path: "/dh/activity" },
        { key: "workload", label: "Workload", icon: "💼", path: "/dh/workload" },
        { key: "performance", label: "Performance", icon: "🔥", path: "/dh/performance" },
        { key: "sla", label: "SLA Overview", icon: "⏳", path: "/dh/sla" },
        { key: "best", label: "Best Technician", icon: "⭐", path: "/dh/technicians/best" },
        { key: "available", label: "Available Techs", icon: "🧑‍🔧", path: "/dh/technicians/available" },

    ];

    const activeKey = (() => {
        if (location.pathname.includes("requests/new")) return "new";
        if (location.pathname.includes("requests/pending")) return "pending";
        if (location.pathname.includes("notifications")) return "notifications";
        if (location.pathname.includes("report")) return "report";
        return "dashboard";
    })();

    return (
        <div className="dh-shell">
            {/* SIDEBAR */}
            <aside className="dh-sidebar">
                <div className="dh-logo">
                    <span className="logo-icon">🏢</span>
                    <div className="logo-text">
                        <div className="logo-title">Dept Head</div>
                        <div className="logo-sub">Facility Request Management</div>
                    </div>
                </div>

                <nav className="dh-menu">
                    {menuItems.map((item) => (
                        <div
                            key={item.key}
                            className={`dh-menu-item ${activeKey === item.key ? "active" : ""}`}
                            onClick={() => navigate(item.path)}
                        >
                            <span className="icon">{item.icon}</span>
                            <span className="label">{item.label}</span>
                        </div>
                    ))}
                </nav>

                <button
                    className="dh-logout-btn"
                    onClick={() => {
                        localStorage.clear();
                        navigate("/login");
                    }}
                >
                    Logout
                </button>
            </aside>

            {/* MAIN CONTENT */}
            <main className="dh-main">
                <Outlet />
            </main>
        </div>
    );
}
