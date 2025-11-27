import { Outlet } from "react-router-dom";


export default function TechnicianLayoutWrapper() {
    return (
        <div className="admin-shell">

            <main className="admin-content">
                <Outlet />
            </main>
        </div>
    );
}
