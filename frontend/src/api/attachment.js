import api from "./api";

const attachment = {
    getByRequest(requestId) {
        return api.get(`/attachment/by-request/${requestId}`);
    },

    create(data) {
        // data: AttachmentCreateDto
        return api.post("/attachment", data);
    },

    delete(id) {
        return api.delete(`/attachment/${id}`);
    },
};

export default attachment;
