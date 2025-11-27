import api from "./api";   // dùng axios chung

const enduser = {
    // 1) Create request
    createRequest: async (payload) => {
        const res = await api.post("/enduser/requests", payload);
        return res.data;
    },

    // 2) My requests
    getMyRequests: async () => {
        const res = await api.get("/enduser/requests/my");
        return res.data;
    },

    // 3) Request detail
    getRequestDetail: async (id) => {
        const res = await api.get(`/enduser/requests/${id}`);
        return res.data;
    },

    // 4) Close request
    closeRequest: async (id, reason) => {
        const res = await api.post(`/enduser/requests/${id}/close`, { reason });
        return res.data;
    },

    // 5) Add comment
    addComment: async (id, body) => {
        const res = await api.post(`/enduser/requests/${id}/comments`, { body });
        return res.data;
    }
};

export default enduser;
