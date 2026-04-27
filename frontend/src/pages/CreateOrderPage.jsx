import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createOrder } from "../api/ordersApi";

const initialForm = {
    citySender: "",
    addressSender: "",
    cityReceiver: "",
    addressReceiver: "",
    weight: "",
    pickUpDate: "",
};

function CreateOrderPage() {
    const [form, setForm] = useState(initialForm);
    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(false);

    const navigate = useNavigate();

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
        setIsLoading(true);

        try {
            const createdOrder = await createOrder({
                citySender: form.citySender,
                addressSender: form.addressSender,
                cityReceiver: form.cityReceiver,
                addressReceiver: form.addressReceiver,
                weight: Number(form.weight),
                pickUpDate: form.pickUpDate,
            });

            navigate(`/orders/${createdOrder.orderNumber}`);
        } catch (error) {
            setError(error.message);
        } finally {
            setIsLoading(false);
        }
    }

    return (
        <section className="card">
            <h1>Создание заказа</h1>

            <form className="form" onSubmit={handleSubmit}>
                <label>
                    Город отправителя
                    <input
                        name="citySender"
                        value={form.citySender}
                        onChange={handleChange}
                        required
                    />
                </label>

                <label>
                    Адрес отправителя
                    <input
                        name="addressSender"
                        value={form.addressSender}
                        onChange={handleChange}
                        required
                    />
                </label>

                <label>
                    Город получателя
                    <input
                        name="cityReceiver"
                        value={form.cityReceiver}
                        onChange={handleChange}
                        required
                    />
                </label>

                <label>
                    Адрес получателя
                    <input
                        name="addressReceiver"
                        value={form.addressReceiver}
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

                <button type="submit" disabled={isLoading}>
                    {isLoading ? "Создание..." : "Создать заказ"}
                </button>
            </form>
        </section>
    );
}

export default CreateOrderPage;