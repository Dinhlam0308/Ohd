import api from "./api";

const requesttag = {
    getByRequest(requestId) {
        return api.get(`/requesttag/by-request/${requestId}`);
    },

    add(data) {
        // data: RequestTagAssignDto
        return api.post("/requesttag/add", data);
    },

    remove(data) {
        // data: RequestTagAssignDto
        return api.post("/requesttag/remove", data);
    },
};

export default requesttag;