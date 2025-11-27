import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../assets/css/admin/AdminDashboard.css";

export default function AdminLayout({ activeTab, setActiveTab, children }) {
    const [isSidebarOpen, setIsSidebarOpen] = useState(true);
    const navigate = useNavigate();

    const menuItems = [
        { key: "overview", label: "Overview", icon: "📊", path: "/admin/dashboard" },
        { key: "users", label: "Users & Roles", icon: "👥", path: "/admin/users" },
        { key: "facilities", label: "Facilities", icon: "🏫", path: "/admin/facilities" },
        { key: "config", label: "System Configuration", icon: "⚙️", path: "/admin/config" },
    ];

    const handleNavigate = (item) => {
        setActiveTab(item.key);
        navigate(item.path);
    };

    // 🟦 LOGOUT HANDLER
    const handleLogout = () => {
        // Xoá token / session frontend
        localStorage.removeItem("token");
        sessionStorage.clear();

        // Điều hướng về trang login
        navigate("/login");
    };

    return (
        <div className="admin-shell">
            {/* SIDEBAR */}
            <aside className={`admin-sidebar ${isSidebarOpen ? "" : "collapsed"}`}>
                <div className="admin-sidebar-header">
                    <div className="logo-circle">A</div>
                    {isSidebarOpen && (
                        <div>
                            <div className="logo-title">OHD Admin</div>
                            <div className="logo-sub">Management Panel</div>
                        </div>
                    )}

                    {/* Toggle button */}
                    <button
                        className="admin-toggle-btn"
                        onClick={() => setIsSidebarOpen(!isSidebarOpen)}
                    >
                        {isSidebarOpen ? "«" : "»"}
                    </button>
                </div>

                {/* Menu */}
                <nav className="admin-menu">
                    {menuItems.map((item) => (
                        <button
                            key={item.key}
                            className={
                                "admin-menu-item " +
                                (activeTab === item.key ? "active" : "")
                            }
                            onClick={() => handleNavigate(item)}
                        >
                            <span className="admin-menu-icon">{item.icon}</span>
                            {isSidebarOpen && <span>{item.label}</span>}
                        </button>
                    ))}
                </nav>
            </aside>

            {/* MAIN */}
            <main className="admin-main">
                {/* HEADER */}
                <header className="admin-header">
                    <div>
                        <div className="admin-header-title">Admin Dashboard</div>
                        <div className="admin-header-sub">
                            System Management & Configuration
                        </div>
                    </div>

                    <div className="admin-header-right">
                        <div className="admin-user-info">
                            <div className="admin-user-name">Administrator</div>
                            <div className="admin-user-role">Super Admin</div>
                        </div>
                        <div className="admin-avatar">A</div>

                        {/* 🔴 LOGOUT BUTTON */}
                        <button className="admin-logout-btn" onClick={handleLogout}>
                            Logout
                        </button>
                    </div>
                </header>

                {/* MAIN CONTENT */}
                <section className="admin-content">{children}</section>
            </main>
        </div>
    );
}
