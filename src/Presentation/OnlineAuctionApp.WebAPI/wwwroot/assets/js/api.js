async function apiRequest(url, options = {}) {
    const response = await fetch(url, {
        credentials: "same-origin",
        headers: {
            "Content-Type": "application/json",
            ...(options.headers || {})
        },
        ...options
    });

    const contentType = response.headers.get("content-type");
    const hasJson = contentType && contentType.includes("application/json");
    const data = hasJson ? await response.json() : null;

    if (!response.ok) {
        const message = data?.message || "Request failed.";
        throw new Error(message);
    }

    return data;
}

function formatPrice(value) {
    return `${Number(value).toFixed(2)} ₼`;
}

function formatShortDate(value) {
    return new Date(value).toLocaleString("en-GB", {
        day: "2-digit",
        month: "short",
        hour: "2-digit",
        minute: "2-digit"
    });
}

function getImageUrl(auction) {
    if (auction.images && auction.images.length > 0) {
        return auction.images[0].imageUrl;
    }

    return null;
}

function calculateCountdown(endTime) {
    const end = new Date(endTime).getTime();
    const now = new Date().getTime();
    const diff = end - now;

    if (diff <= 0) {
        return "Ended";
    }

    const days = Math.floor(diff / (1000 * 60 * 60 * 24));
    const hours = Math.floor((diff / (1000 * 60 * 60)) % 24);
    const minutes = Math.floor((diff / (1000 * 60)) % 60);

    if (days > 0) {
        return `${days}d ${hours}h left`;
    }

    if (hours > 0) {
        return `${hours}h ${minutes}m left`;
    }

    return `${minutes}m left`;
}