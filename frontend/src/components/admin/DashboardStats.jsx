// src/components/admin/DashboardStats.jsx
import {
    ClipboardDocumentListIcon,
    ClockIcon,
    ExclamationTriangleIcon,
    UserGroupIcon,
    BuildingOfficeIcon,
} from "@heroicons/react/24/outline";

export default function AdminStatCards({ stats }) {
    const cards = [
        {
            title: "Total Requests",
            value: stats.totalRequests,
            sub: "All requests in the system",
            icon: <ClipboardDocumentListIcon className="w-12 h-12 text-blue-300 drop-shadow-md" />,
            glow: "shadow-[0_0_25px_rgba(96,165,250,0.5)]",
            border: "border-blue-400/40",
            gradient: "from-blue-900/40 to-blue-700/10",
        },
        {
            title: "Open Requests",
            value: stats.openRequests,
            sub: "Pending and not completed",
            icon: <ClockIcon className="w-12 h-12 text-amber-300 drop-shadow-md" />,
            glow: "shadow-[0_0_25px_rgba(251,191,36,0.5)]",
            border: "border-amber-400/40",
            gradient: "from-amber-900/40 to-amber-700/10",
        },
        {
            title: "Overdue Requests",
            value: stats.overdueRequests,
            sub: "Require urgent attention",
            icon: <ExclamationTriangleIcon className="w-12 h-12 text-red-300 drop-shadow-md" />,
            glow: "shadow-[0_0_25px_rgba(248,113,113,0.5)]",
            border: "border-red-400/40",
            gradient: "from-red-900/40 to-red-700/10",
        },
        {
            title: "Users",
            value: stats.totalUsers,
            sub: "Active user accounts",
            icon: <UserGroupIcon className="w-12 h-12 text-green-300 drop-shadow-md" />,
            glow: "shadow-[0_0_25px_rgba(74,222,128,0.5)]",
            border: "border-green-400/40",
            gradient: "from-green-900/40 to-green-700/10",
        },
        {
            title: "Facilities",
            value: stats.totalFacilities,
            sub: "Managed facilities",
            icon: <BuildingOfficeIcon className="w-12 h-12 text-purple-300 drop-shadow-md" />,
            glow: "shadow-[0_0_25px_rgba(216,180,254,0.5)]",
            border: "border-purple-400/40",
            gradient: "from-purple-900/40 to-purple-700/10",
        },
    ];

    return (
        <div className="grid gap-6 md:grid-cols-2 xl:grid-cols-3">
            {cards.map((c, idx) => (
                <div
                    key={idx}
                    className={`
                        rounded-2xl px-6 py-5 
                        bg-gradient-to-br ${c.gradient}
                        border ${c.border}
                        backdrop-blur-xl
                        transition-all duration-300
                        hover:scale-[1.05]
                        hover:shadow-2xl hover:${c.glow}
                        shadow-lg
                    `}
                >
                    <div className="flex items-center justify-between">
                        <div className="space-y-1">
                            <div className="text-lg font-semibold tracking-wide text-white/90">
                                {c.title}
                            </div>
                            <div className="text-4xl font-bold text-white">
                                {c.value ?? 0}
                            </div>
                            <div className="text-sm text-white/60">{c.sub}</div>
                        </div>

                        <div className="opacity-90 transform hover:scale-110 transition">
                            {c.icon}
                        </div>
                    </div>
                </div>
            ))}
        </div>
    );
}
