import api from "./api";

const auth = {
    login(data) {
        // data: { email, password }
        return api.post("/auth/login", data);
    },

    changePasswordFirstLogin(data) {
        // data: { userId, oldPassword, newPassword }
        return api.post("/auth/change-password-first-login", data);
    },

    forgotPassword(email) {
        return api.post("/auth/forgot-password", { email });
    },

    resetPassword(data) {
        // data: { token, newPassword, confirmNewPassword }
        return api.post("/auth/reset-password", data);
    },
    googleLogin(data) {
        return api
            .post("/auth/google-login", data)
            .then((res) => res.data);
    }

};

export default auth;
