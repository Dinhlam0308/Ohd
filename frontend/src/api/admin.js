// src/api/admin.js
import api from "./api";

const admin = {
    // ======================
    // DASHBOARD
    // ======================
    getDashboard() {
        return api.get("/admin/dashboard").then((res) => res.data);
    },

    // ======================
    // USERS (Pagination + Search)
    // ======================
    getUsers({ page = 1, pageSize = 10, search = "" } = {}) {
        return api.get("/admin/users", {
            params: { page, pageSize, search }
        }).then((res) => res.data);
    },

    createUser(data) {
        return api.post("/admin/users", data).then((res) => res.data);
    },

    updateUser(userId, data) {
        return api.put(`/admin/users/${userId}`, data).then((res) => res.data);
    },

    changeUserRole(userId, roleId) {
        return api
            .put(`/admin/users/${userId}/role/${roleId}`)
            .then((res) => res.data);
    },

    toggleUserActive(userId, isActive) {
        return api
            .put(`/admin/users/${userId}/toggle-active`, { isActive })
            .then((res) => res.data);
    },

    deleteUser(userId) {
        return api.delete(`/admin/users/${userId}`).then((res) => res.data);
    },

    importUsersFromExcel(file) {
        const formData = new FormData();
        formData.append("file", file);
        return api
            .post("/admin/users/import-excel", formData, {
                headers: { "Content-Type": "multipart/form-data" },
            })
            .then((res) => res.data);
    },

    // ======================
    // FACILITIES
    // ======================
    getFacilities({ page = 1, pageSize = 10, search = "" } = {}) {
        return api
            .get("/admin/facilities", { params: { page, pageSize, search } })
            .then((res) => res.data);
    },

    createFacility(payload) {
        return api.post("/admin/facilities", payload);
    },

    updateFacility(id, payload) {
        return api.put(`/admin/facilities/${id}`, payload);
    },

    deleteFacility(id) {
        return api.delete(`/admin/facilities/${id}`);
    },

    // =======================
    // TECHNICIANS
    // =======================
    getTechnicians() {
        return api
            .get("/admin/facilities/technicians")
            .then((res) => res.data);
    },

    // ======================
    // REQUESTS (Pagination)
    // ======================
    getRequests({
                    statusId,
                    severityId,
                    facilityId,
                    assigneeId,
                    search,
                    page = 1,
                    pageSize = 10
                } = {}) {
        return api.get("/admin/requests", {
            params: {
                statusId,
                severityId,
                facilityId,
                assigneeId,
                search,
                page,
                pageSize
            }
        }).then((res) => res.data);
    },

    assignRequest(id, assigneeId) {
        return api
            .put(`/admin/requests/${id}/assign/${assigneeId}`)
            .then((res) => res.data);
    },

    changeRequestStatus(id, statusId) {
        return api
            .put(`/admin/requests/${id}/status/${statusId}`)
            .then((res) => res.data);
    },

    closeRequest(id) {
        return api.put(`/admin/requests/${id}/close`).then((res) => res.data);
    },

    // ======================
    // NOTIFICATIONS
    // ======================
    getNotifications() {
        return api.get("/admin/notifications").then((res) => res.data);
    },

    sendNotificationForRequest(requestId) {
        return api
            .post(`/admin/notifications/${requestId}/send`)
            .then((res) => res.data);
    },

    // ======================
    // CONFIG
    // ======================
    getSeverities() {
        return api.get("/admin/config/severities").then((res) => res.data);
    },

    createSeverity(data) {
        return api.post("/admin/config/severities", data).then((res) => res.data);
    },

    updateSeverity(id, data) {
        return api.put(`/admin/config/severities/${id}`, data).then((res) => res.data);
    },

    deleteSeverity(id) {
        return api.delete(`/admin/config/severities/${id}`).then((res) => res.data);
    },

    getRequestStatuses(filters = {}) {
        return api
            .get("/admin/config/statuses", { params: filters })
            .then((res) => res.data);
    },

    createRequestStatus(data) {
        return api.post("/admin/config/statuses", data).then((res) => res.data);
    },

    updateRequestStatus(id, data) {
        return api.put(`/admin/config/statuses/${id}`, data).then((res) => res.data);
    },

    deleteRequestStatus(id) {
        return api.delete(`/admin/config/statuses/${id}`).then((res) => res.data);
    },

    sendOverdueNotifications(statusId) {
        return api
            .post(`/admin/config/statuses/${statusId}/send-overdue-notifications`)
            .then((res) => res.data);
    },

    getCategories() {
        return api.get("/admin/config/categories").then((res) => res.data);
    },

    createCategory(data) {
        return api.post("/admin/config/categories", data).then((res) => res.data);
    },

    updateCategory(id, data) {
        return api.put(`/admin/config/categories/${id}`, data).then((res) => res.data);
    },

    deleteCategory(id) {
        return api.delete(`/admin/config/categories/${id}`).then((res) => res.data);
    },

    getSettings() {
        return api.get("/admin/config/settings").then((res) => res.data);
    },

    getSetting(key) {
        return api
            .get(`/admin/config/settings/${encodeURIComponent(key)}`)
            .then((res) => res.data);
    },

    upsertSetting(data) {
        return api.post("/admin/config/settings", data).then((res) => res.data);
    },

    deleteSetting(key) {
        return api
            .delete(`/admin/config/settings/${encodeURIComponent(key)}`)
            .then((res) => res.data);
    },

    getRoles() {
        return api.get("/admin/config/roles").then((res) => res.data);
    },

    createRole(data) {
        return api.post("/admin/config/roles", data).then((res) => res.data);
    },

    updateRole(id, data) {
        return api.put(`/admin/config/roles/${id}`, data).then((res) => res.data);
    },

    deleteRole(id) {
        return api.delete(`/admin/config/roles/${id}`).then((res) => res.data);
    },

    downloadUsersExcel() {
        return api.get("/admin/users/export-excel", {
            responseType: "blob"
        });
    },

    importUsersFromExcel(file) {
        const formData = new FormData();
        formData.append("file", file);

        return api.post("/admin/users/import-excel", formData, {
            headers: { "Content-Type": "multipart/form-data" },
        }).then((res) => res.data);
    },

    importUsersFromExcelUpdate(file) {
        const formData = new FormData();
        formData.append("file", file);

        return api.post("/admin/users/import-excel-update", formData, {
            headers: { "Content-Type": "multipart/form-data" },
        }).then((res) => res.data);
    },

    // ======================
    // AUDIT LOGS (Pagination)
    // ======================
    getAuditLogs({
                     userId,
                     action,
                     from,
                     to,
                     page = 1,
                     pageSize = 20
                 } = {}) {
        return api
            .get("/admin/audit-logs", {
                params: { userId, action, from, to, page, pageSize }
            })
            .then((res) => res.data);
    },

    getAuditLogActions() {
        return api.get("/admin/audit-logs/actions").then((res) => res.data);
    },
};

export default admin;
