// src/api/technician.js
import api from "./api";

const technicianApi = {
    // ===============================
    // 1) LẤY REQUEST ĐƯỢC GÁN
    // ===============================
    getMyRequests(params = {}) {
        return api.get("/technician/requests", { params })
            .then(res => res.data);
    },

    // ===============================
    // 2) UPDATE STATUS
    // ===============================
    updateStatus(id, { statusId, note }) {
        return api.put(`/technician/requests/${id}/status`, {
            statusId,
            note: note || ""
        }).then(res => res.data);
    },

    // ===============================
    // 3) ADD COMMENT
    // ===============================
    addComment(id, body) {
        return api.post(`/technician/requests/${id}/comments`, {
            body
        }).then(res => res.data);
    },

    // ===============================
    // 4) GET DETAIL + TIMELINE
    // ===============================
    getDetail(id) {
        return api.get(`/technician/requests/${id}`)
            .then(res => res.data);
    },

    // ===============================
    // 5) UPLOAD IMAGE (Cloudinary)
    // ===============================
    uploadImages(id, files) {
        const form = new FormData();
        for (const f of files) form.append("files", f);

        return api.post(`/technician/requests/${id}/upload`, form, {
            headers: { "Content-Type": "multipart/form-data" }
        }).then(res => res.data);
    }
};

export default technicianApi;
