import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { deleteOrder, getOrders } from "../api/ordersApi";

function OrdersListPage() {
    const [orders, setOrders] = useState([]);
    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(true);

    async function loadOrders() {
        try {
            const data = await getOrders();
            setOrders(data);
        } catch (error) {
            setError(error.message);
        } finally {
            setIsLoading(false);
        }
    }

    async function handleDelete(orderNumber) {
        const isConfirmed = confirm(`Удалить заказ №${orderNumber}?`);

        if (!isConfirmed) {
            return;
        }

        try {
            await deleteOrder(orderNumber);

            // Перезапрашиваем список с сервера
            await loadOrders();
        } catch (error) {
            setError(error.message);
        }
    }

    useEffect(() => {
        loadOrders();
    }, []);

    if (isLoading) {
        return <p>Загрузка заказов...</p>;
    }

    if (error) {
        return <p className="error">{error}</p>;
    }

    return (
        <section className="card">
            <div className="page-header">
                <h1>Список заказов</h1>

                <Link to="/create" className="button-link">
                    Создать заказ
                </Link>
            </div>

            {orders.length === 0 ? (
                <p>Заказов пока нет.</p>
            ) : (
                <table className="table">
                    <thead>
                    <tr>
                        <th>Номер</th>
                        <th>Откуда</th>
                        <th>Куда</th>
                        <th>Вес</th>
                        <th>Дата забора</th>
                        <th>Действия</th>
                    </tr>
                    </thead>

                    <tbody>
                    {orders.map((order) => (
                        <tr key={order.orderNumber}>
                            <td>№{order.orderNumber}</td>
                            <td>
                                {order.citySender}, {order.addressSender}
                            </td>
                            <td>
                                {order.cityReceiver}, {order.addressReceiver}
                            </td>
                            <td>{order.weight}</td>
                            <td>{order.pickUpDate}</td>
                            <td className="actions">
                                <Link to={`/orders/${order.orderNumber}`}>
                                    Открыть
                                </Link>

                                <button
                                    className="danger-button"
                                    type="button"
                                    onClick={() => handleDelete(order.orderNumber)}
                                >
                                    Удалить
                                </button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            )}
        </section>
    );
}

export default OrdersListPage;