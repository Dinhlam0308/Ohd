import { useEffect, useState } from "react";
import requestApi from "../../api/request";
import {
    BarChart,
    Bar,
    XAxis,
    YAxis,
    Tooltip,
    CartesianGrid,
    ResponsiveContainer
} from "recharts";

export default function OverdueChart() {
    const [data, setData] = useState([]);

    useEffect(() => {
        const load = async () => {
            const res = await requestApi.getOverdueList();
            const list = res.data;

            const groups = {};

            list.forEach((r) => {
                const key = r.dueDate.slice(0, 10);
                groups[key] = (groups[key] || 0) + 1;
            });

            const chartData = Object.keys(groups)
                .sort()
                .map(date => ({
                    date,
                    count: groups[date]
                }));

            setData(chartData);
        };

        load();
    }, []);

    return (
        <div className="admin-panel">
            <div className="admin-panel-header">
                <h2 className="admin-panel-title">📊 Overdue Bar Chart</h2>
                <p className="admin-panel-sub">Daily overdue request counts</p>
            </div>

            <div style={{ width: "100%", height: 350 }}>
                <ResponsiveContainer>
                    <BarChart data={data}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="date" stroke="#ccc" />
                        <YAxis stroke="#ccc" />
                        <Tooltip />
                        <Bar dataKey="count" fill="#ef4444" />
                    </BarChart>
                </ResponsiveContainer>
            </div>
        </div>
    );
}
