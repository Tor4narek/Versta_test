const API_URL = import.meta.env.VITE_API_URL;

export async function createOrder(order) {
    const response = await fetch(`${API_URL}/api/orders`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(order),
    });

    if (!response.ok) {
        throw new Error("Ошибка при создании заказа");
    }

    return await response.json();
}

export async function getOrders() {
    const response = await fetch(`${API_URL}/api/orders`);

    if (!response.ok) {
        throw new Error("Ошибка при загрузке заказов");
    }

    return await response.json();
}

export async function getOrderByNumber(orderNumber) {
    const response = await fetch(`${API_URL}/api/orders/${orderNumber}`);

    if (!response.ok) {
        throw new Error("Заказ не найден");
    }

    return await response.json();
}

export async function updateOrder(orderNumber, order) {
    const response = await fetch(`${API_URL}/api/orders/${orderNumber}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(order),
    });

    if (!response.ok) {
        throw new Error("Ошибка при обновлении заказа");
    }

    return await response.json();
}

export async function deleteOrder(orderNumber) {
    const response = await fetch(`${API_URL}/api/orders/${orderNumber}`, {
        method: "DELETE",
    });

    if (!response.ok) {
        throw new Error("Ошибка при удалении заказа");
    }
}