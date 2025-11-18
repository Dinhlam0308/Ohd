import api from "./api";

const requestcomment = {
    getByRequest(requestId) {
        return api.get(`/requestcomment/by-request/${requestId}`);
    },

    create(data) {
        // data: RequestCommentCreateDto
        return api.post("/requestcomment", data);
    },

    update(id, data) {
        // data: RequestCommentUpdateDto
        return api.put(`/requestcomment/${id}`, data);
    },

    delete(id) {
        return api.delete(`/requestcomment/${id}`);
    },
};

export default requestcomment;
