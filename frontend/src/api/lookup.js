import api from "./api";

const lookup = {
    getRequestStatuses() {
        return api.get("/lookups/request-statuses");
    },

    getSeverities() {
        return api.get("/lookups/severities");
    },

    getRequestPriorities() {
        return api.get("/lookups/priorities");
    },

    getFacilities() {
        return api.get("/lookups/facilities");
    },

    getTags() {
        return api.get("/lookups/tags");
    },

    getSkills() {
        return api.get("/lookups/skills");
    },

    getTeams() {
        return api.get("/lookups/teams");
    },

    getSlaPolicies() {
        return api.get("/lookups/sla-policies");
    },

    getMaintenanceWindows() {
        return api.get("/lookups/maintenance-windows");
    },

    getSystemSettings() {
        return api.get("/lookups/system-settings");
    },
};

export default lookup;
