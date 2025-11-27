export default function CommentList({ comments }) {
    if (!comments) return null;

    return (
        <div className="space-y-3">
            {comments.map((c) => (
                <div key={c.id} className="p-3 border rounded bg-gray-50">
                    <p>{c.body}</p>
                    <p className="text-xs text-gray-500">
                        {new Date(c.createdAt).toLocaleString()}
                    </p>
                </div>
            ))}
        </div>
    );
}
