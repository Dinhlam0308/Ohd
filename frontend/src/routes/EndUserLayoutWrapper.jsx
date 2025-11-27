import { Outlet, NavLink } from "react-router-dom";

export default function EndUserLayoutWrapper() {
    return (
        <div className="w-[100vw] h-[100vh] flex bg-gray-100 text-black">

            {/* SIDEBAR */}
            <aside className="w-64 bg-white shadow-lg border-r border-gray-300 h-full">
                <div className="p-6 border-b border-gray-300">
                    <h1 className="text-2xl font-bold text-indigo-700">
                        Help Desk
                    </h1>
                    <p className="text-gray-700 text-sm">End-User Portal</p>
                </div>

                <nav className="mt-4 space-y-1 text-black">
                    <NavLink
                        to="/eu/requests"
                        className={({ isActive }) =>
                            `block px-6 py-3 font-medium rounded ${
                                isActive
                                    ? "bg-indigo-100 text-indigo-700"
                                    : "hover:bg-gray-200"
                            }`
                        }
                    >
                        📄 My Requests
                    </NavLink>

                    <NavLink
                        to="/eu/requests/create"
                        className={({ isActive }) =>
                            `block px-6 py-3 font-medium rounded ${
                                isActive
                                    ? "bg-indigo-100 text-indigo-700"
                                    : "hover:bg-gray-200"
                            }`
                        }
                    >
                        ➕ Create Request
                    </NavLink>
                </nav>
            </aside>

            {/* MAIN CONTENT */}
            <main className="flex-1 p-10 overflow-y-auto text-black">
                <Outlet />
            </main>
        </div>
    );
}
