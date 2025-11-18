import api from "./api";

const admin = {
    createUser(data) {
        // data: AdminCreateUserRequest
        return api.post("/admin/users", data);
    },
};

export default admin;
