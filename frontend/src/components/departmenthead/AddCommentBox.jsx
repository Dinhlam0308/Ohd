import { useState } from "react";
import departmenthead from "../../api/departmenthead";

export default function AddCommentBox({ requestId, onAdded }) {
    const [text, setText] = useState("");

    const submit = async () => {
        if (!text.trim()) return;
        await departmenthead.addComment(requestId, { message: text });
        setText("");
        onAdded();
    };

    return (
        <div className="p-4 bg-gray-50 rounded-xl border mt-4">
            <textarea
                className="textarea textarea-bordered w-full"
                rows="3"
                placeholder="Add a comment..."
                value={text}
                onChange={(e) => setText(e.target.value)}
            />

            <button className="btn btn-primary mt-3 w-full" onClick={submit}>
                Add Comment
            </button>
        </div>
    );
}
