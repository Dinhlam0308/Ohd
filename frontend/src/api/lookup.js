import api from "./api";

const lookup = {
    getRequestStatuses: async () => {
        const res = await api.get("/lookups/request-statuses");
        return res.data;
    },

    getSeverities: async () => {
        const res = await api.get("/lookups/severities");
        return res.data;
    },

    getRequestPriorities: async () => {
        const res = await api.get("/lookups/priorities");
        return res.data;
    },

    getFacilities: async () => {
        const res = await api.get("/lookups/facilities");
        return res.data;
    },

    getTags: async () => {
        const res = await api.get("/lookups/tags");
        return res.data;
    },

    getSkills: async () => {
        const res = await api.get("/lookups/skills");
        return res.data;
    },

    getTeams: async () => {
        const res = await api.get("/lookups/teams");
        return res.data;
    },

    getSlaPolicies: async () => {
        const res = await api.get("/lookups/sla-policies");
        return res.data;
    },

    getMaintenanceWindows: async () => {
        const res = await api.get("/lookups/maintenance-windows");
        return res.data;
    },

    getSystemSettings: async () => {
        const res = await api.get("/lookups/system-settings");
        return res.data;
    },
};

export default lookup;
