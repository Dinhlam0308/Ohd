export default function CommentBox({ comment, setComment, onSubmit }) {
    return (
        <div>
            <textarea
                className="w-full border p-2 rounded"
                placeholder="Write a comment..."
                value={comment}
                onChange={(e) => setComment(e.target.value)}
            />

            <button
                onClick={onSubmit}
                className="mt-2 bg-blue-600 text-white px-4 py-2 rounded"
            >
                Add Comment
            </button>
        </div>
    );
}
