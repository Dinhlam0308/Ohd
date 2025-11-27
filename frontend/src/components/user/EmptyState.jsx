export default function EmptyState({ message = "No data available." }) {
    return (
        <div className="p-6 text-center text-gray-500">
            <div className="text-5xl mb-3">📭</div>
            <p className="text-lg">{message}</p>
        </div>
    );
}
