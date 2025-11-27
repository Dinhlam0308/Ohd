import { Outlet } from "react-router-dom";
import TechnicianSidebar from "../../components/technician/TechnicianSidebar.jsx";

export default function TechnicianLayout() {
    return (
        <div className="flex h-screen bg-slate-100">
            <TechnicianSidebar />

            <main className="flex-1 overflow-y-auto p-6">
                <Outlet />
            </main>
        </div>
    );
}