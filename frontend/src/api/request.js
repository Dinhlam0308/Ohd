import api from "./api";

const request = {
    getAll() {
        return api.get("/request");
    },

    getById(id) {
        return api.get(`/request/${id}`);
    },

    create(data) {
        // data: RequestCreateDto
        return api.post("/request", data);
    },

    update(id, data) {
        // data: RequestUpdateDto
        return api.put(`/request/${id}`, data);
    },

    changeStatus(id, data) {
        // data: RequestChangeStatusDto
        return api.put(`/request/${id}/status`, data);
    },

    delete(id) {
        return api.delete(`/request/${id}`);
    },
};

export default request;
