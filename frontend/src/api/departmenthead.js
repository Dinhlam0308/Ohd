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

    // Đổi người xử lý (reassign)
    reassignRequest(data) {
        // data: { requestId, newAssigneeId }
        return api.put("/departmenthead/request/reassign", data);
    },

    // Đóng yêu cầu
    closeRequest(id) {
        return api.put(`/departmenthead/request/${id}/close`);
    },

    // Mở lại yêu cầu
    reopenRequest(id) {
        return api.put(`/departmenthead/request/${id}/reopen`);
    },

    // Chi tiết yêu cầu
    getRequestDetail(id) {
        return api.get(`/departmenthead/request/${id}`);
    },

    // Timeline xử lý của yêu cầu
    getRequestTimeline(id) {
        return api.get(`/departmenthead/request/${id}/timeline`);
    },

    // Tìm kiếm / filter request (priority, status, facility, technician, date range, ...)
    searchRequests(params) {
        return api.get("/departmenthead/requests/search", { params });
    },

    // Thêm comment / ghi chú vào request
    addComment(requestId, data) {
        // data: { message, isInternal, ... }
        return api.post(`/departmenthead/request/${requestId}/comment`, data);
    },


    // ============================
    // 2. Technicians
    // ============================
    // Danh sách technician
    getTechnicians() {
        return api.get("/departmenthead/technicians");
    },

    // Thống kê theo từng technician (KPI, số ticket, SLA, ...)
    getTechnicianStats(id) {
        return api.get(`/departmenthead/technician/${id}/stats`);
    },

    // Chỉ lấy những technician rảnh trong khung giờ của request
    getAvailableTechnicians(requestId, dateTime) {
        // dateTime: ISO string, ví dụ "2025-11-26T14:00:00"
        return api.get("/departmenthead/technicians/available", {
            params: { requestId, dateTime },
        });
    },

    // Lấy technician phù hợp nhất xử lý request (đã sắp xếp theo workload/SLA/…)
    getBestTechnicianForRequest: (requestId) =>
        api.get(`/departmenthead/technicians/best?requestId=${requestId}`),



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

    // Workload / SLA / Activity / Performance
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


    // ============================
    // 5. Export
    // ============================
    exportReport(format, scope) {
        // format: "xlsx" | "pdf" | ...
        // scope: { fromDate, toDate, facilityId, ... }
        return api.post(
            "/departmenthead/export",
            { format, scope },
            { responseType: "blob" }
        );
    },


    // ============================
    // 6. Bulk assign
    // ============================
    getBulkRequests() {
        return api.get("/departmenthead/bulk/requests");
    },

    bulkAssign(data) {
        // data: { requestIds: long[], assigneeId: long }
        return api.post("/departmenthead/bulk/assign", data);
    }
};

export default departmenthead;
