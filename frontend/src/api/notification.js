import api from "./api";

const notification = {
    getByUser(userId) {
        return api.get(`/notification/by-user/${userId}`);
    },

    create(data) {
        // data: NotificationCreateDto
        return api.post("/notification", data);
    },

    markRead(id) {
        return api.put(`/notification/mark-read/${id}`);
    },
};

export default notification;
