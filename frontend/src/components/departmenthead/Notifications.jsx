import { useEffect, useState } from "react";
import departmentheadApi from "../../api/departmenthead";

export default function Notifications() {
    const [list, setList] = useState([]);

    const load = () => {
        departmentheadApi.getNotifications().then(res => setList(res.data));
    };

    useEffect(() => {
        load();
    }, []);

    return (
        <>
            <div className="dh-page-title">🔔 Notifications</div>
            <div className="dh-page-sub">System notifications sent to you</div>

            <div className="dh-noti-list">
                {list.map((n) => (
                    <div key={n.id} className={`dh-noti ${n.is_read ? "read" : ""}`}>
                        <div>
                            <div className="dh-noti-msg">{n.message}</div>
                            <div className="dh-noti-time">
                                {new Date(n.created_at).toLocaleString()}
                            </div>
                        </div>

                        {!n.is_read && (
                            <button
                                className="dh-noti-btn"
                                onClick={async () => {
                                    await departmentheadApi.markNotificationAsRead(n.id);
                                    load();
                                }}
                            >
                                Mark as read
                            </button>
                        )}
                    </div>
                ))}
            </div>
        </>
    );
}
