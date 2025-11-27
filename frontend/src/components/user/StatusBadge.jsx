export default function StatusBadge({ name, color }) {
    return (
        <span
            className="px-2 py-1 text-white rounded"
            style={{ backgroundColor: color }}
        >
            {name}
        </span>
    );
}
