import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import {
    deleteOrder,
    getOrderByNumber,
    updateOrder,
} from "../api/ordersApi";

function OrderDetailsPage() {
    const { orderNumber } = useParams();
    const navigate = useNavigate();

    const [form, setForm] = useState(null);
    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(true);
    const [isSaving, setIsSaving] = useState(false);

    useEffect(() => {
        async function loadOrder() {
            try {
                const data = await getOrderByNumber(orderNumber);
                setForm(data);
            } catch (error) {
                setError(error.message);
            } finally {
                setIsLoading(false);
            }
        }

        loadOrder();
    }, [orderNumber]);

    function handleChange(event) {
        const { name, value } = event.target;

        setForm((prev) => ({
            ...prev,
            [name]: value,
        }));
    }

    async function handleSubmit(event) {
        event.preventDefault();

        setError("");
        setIsSaving(true);

        try {
            const updatedOrder = await updateOrder(orderNumber, {
                citySender: form.citySender,
                addressSender: form.addressSender,
                cityReceiver: form.cityReceiver,
                addressReceiver: form.addressReceiver,
                weight: Number(form.weight),
                pickUpDate: form.pickUpDate,
            });

            setForm(updatedOrder);
        } catch (error) {
            setError(error.message);
        } finally {
            setIsSaving(false);
        }
    }

    async function handleDelete() {
        const isConfirmed = confirm(`Удалить заказ №${orderNumber}?`);

        if (!isConfirmed) {
            return;
        }

        try {
            await deleteOrder(orderNumber);
            navigate("/");
        } catch (error) {
            setError(error.message);
        }
    }

    if (isLoading) {
        return <p>Загрузка заказа...</p>;
    }

    if (error && !form) {
        return <p className="error">{error}</p>;
    }

    if (!form) {
        return <p>Заказ не найден.</p>;
    }

    return (
        <section className="card">
            <Link to="/">← Назад к списку</Link>

            <div className="page-header">
                <h1>Заказ №{form.orderNumber}</h1>

                <button
                    className="danger-button"
                    type="button"
                    onClick={handleDelete}
                >
                    Удалить
                </button>
            </div>

            <form className="form" onSubmit={handleSubmit}>
                <label>
                    Город отправителя
                    <input
                        name="citySender"
                        value={form.citySender ?? ""}
                        onChange={handleChange}
                        required
                    />
                </label>

                <label>
                    Адрес отправителя
                    <input
                        name="addressSender"
                        value={form.addressSender ?? ""}
                        onChange={handleChange}
                        required
                    />
                </label>

                <label>
                    Город получателя
                    <input
                        name="cityReceiver"
                        value={form.cityReceiver ?? ""}
                        onChange={handleChange}
                        required
                    />
                </label>

                <label>
                    Адрес получателя
                    <input
                        name="addressReceiver"
                        value={form.addressReceiver ?? ""}
                        onChange={handleChange}
                        required
                    />
                </label>

                <label>
                    Вес
                    <input
                        name="weight"
                        type="number"
                        min="0.1"
                        step="0.1"
                        value={form.weight}
                        onChange={handleChange}
                        required
                    />
                </label>

                <label>
                    Дата забора груза
                    <input
                        name="pickUpDate"
                        type="date"
                        value={form.pickUpDate}
                        onChange={handleChange}
                        required
                    />
                </label>

                {error && <p className="error">{error}</p>}

                <button type="submit" disabled={isSaving}>
                    {isSaving ? "Сохранение..." : "Сохранить изменения"}
                </button>
            </form>
        </section>
    );
}

export default OrderDetailsPage;