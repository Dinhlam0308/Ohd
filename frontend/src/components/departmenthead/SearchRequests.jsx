import { useState } from "react";
import departmenthead from "../../api/departmenthead";

export default function SearchRequests({ onResults }) {
    const [params, setParams] = useState({
        statusId: "",
        priority: "",
        assigneeId: "",
        fromDate: "",
        toDate: "",
    });

    const search = async () => {
        const res = await departmenthead.searchRequests(params);
        onResults(res.data);
    };

    return (
        <div className="p-4 bg-white border rounded-xl mb-4">
            <h3 className="font-bold mb-3">Search Requests</h3>

            <div className="grid grid-cols-2 gap-3">
                <input className="input input-bordered" placeholder="Status ID"
                       onChange={(e) => setParams({ ...params, statusId: e.target.value })} />

                <input className="input input-bordered" placeholder="Priority ID"
                       onChange={(e) => setParams({ ...params, priority: e.target.value })} />

                <input className="input input-bordered" placeholder="Assignee ID"
                       onChange={(e) => setParams({ ...params, assigneeId: e.target.value })} />

                <input type="date" className="input input-bordered"
                       onChange={(e) => setParams({ ...params, fromDate: e.target.value })} />

                <input type="date" className="input input-bordered"
                       onChange={(e) => setParams({ ...params, toDate: e.target.value })} />
            </div>

            <button className="btn btn-primary mt-4 w-full" onClick={search}>
                Search
            </button>
        </div>
    );
}
