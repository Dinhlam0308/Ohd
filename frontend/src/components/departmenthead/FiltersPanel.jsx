// src/components/departmenthead/FilterPanel.jsx
import React from "react";

export default function FilterPanel({ filters, onChange }) {
    const handleChange = (field, value) => {
        onChange({
            ...filters,
            [field]: value,
        });
    };

    return (
        <div className="dh-filter-panel">
            <div className="dh-filter-group">
                <label className="dh-filter-label">Search</label>
                <input
                    type="text"
                    className="dh-filter-input"
                    placeholder="Search by user, action, request id..."
                    value={filters.search || ""}
                    onChange={(e) => handleChange("search", e.target.value)}
                />
            </div>

            <div className="dh-filter-group">
                <label className="dh-filter-label">From date</label>
                <input
                    type="date"
                    className="dh-filter-input"
                    value={filters.from || ""}
                    onChange={(e) => handleChange("from", e.target.value)}
                />
            </div>

            <div className="dh-filter-group">
                <label className="dh-filter-label">To date</label>
                <input
                    type="date"
                    className="dh-filter-input"
                    value={filters.to || ""}
                    onChange={(e) => handleChange("to", e.target.value)}
                />
            </div>

            <div className="dh-filter-group">
                <label className="dh-filter-label">Status (optional)</label>
                <select
                    className="dh-filter-input"
                    value={filters.status || ""}
                    onChange={(e) => handleChange("status", e.target.value)}
                >
                    <option value="">All</option>
                    <option value="new">New</option>
                    <option value="assigned">Assigned</option>
                    <option value="completed">Completed</option>
                    <option value="overdue">Overdue</option>
                </select>
            </div>
        </div>
    );
}
