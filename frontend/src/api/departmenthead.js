// src/api/departmenthead.js
import api from "./api";

const departmenthead = {
    // ============================
    // 1. Requests
    // ============================

    // Lấy yêu cầu mới
    getNewRequests() {
        return api.get("/departmenthead/requests/new");
    },

    // Lấy yêu cầu pending (Assigned)
    getPendingRequests() {
        return api.get("/departmenthead/requests/pending");
    },

    // Gán request cho technician
    assignRequest(data) {
        // data: { requestId, assigneeId }
        return api.post("/departmenthead/assign", data);
    },


    // ============================
    // 2. Technicians
    // ============================
    getTechnicians() {
        return api.get("/departmenthead/technicians");
    },


    // ============================
    // 3. Notifications
    // ============================
    getNotifications() {
        return api.get("/departmenthead/notifications");
    },

    markNotificationAsRead(id) {
        return api.put(`/departmenthead/notifications/${id}/read`);
    },


    // ============================
    // 4. Reports & Dashboard
    // ============================
    getMonthlyReport() {
        return api.get("/departmenthead/report/monthly");
    },

    getDashboardStats() {
        return api.get("/departmenthead/dashboard");
    },
    getRequestDetail(id) {
        return api.get(`/departmenthead/request/${id}`);
    },
    getRequestTimeline(id) {
        return api.get(`/departmenthead/request/${id}/timeline`);
    },

    // ⭐ NEW — Workload / SLA / Activity / Performance
    getWorkload() {
        return api.get("/departmenthead/workload");
    },
    getSLAOverview() {
        return api.get("/departmenthead/sla");
    },
    getActivityLog() {
        return api.get("/departmenthead/activity");
    },
    getPerformance() {
        return api.get("/departmenthead/performance");
    },

    // ⭐ NEW — Export
    exportReport(format, scope) {
        return api.post(
            "/departmenthead/export",
            { format, scope },
            { responseType: "blob" }
        );
    },

    // ⭐ NEW — Bulk assign
    getBulkRequests() {
        return api.get("/departmenthead/bulk/requests");
    },
    bulkAssign(data) {
        // data: { requestIds: long[], assigneeId: long }
        return api.post("/departmenthead/bulk/assign", data);
    },
};

export default departmenthead;
