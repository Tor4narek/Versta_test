import { Link, Route, Routes } from "react-router-dom";
import CreateOrderPage from "./pages/CreateOrderPage";
import OrdersListPage from "./pages/OrdersListPage";
import OrderDetailsPage from "./pages/OrderDetailsPage";

function App() {
  return (
      <div className="app">
        <header className="header">
          <Link to="/" className="logo">
            Delivery Orders
          </Link>

          <nav className="nav">
            <Link to="/">Список заказов</Link>
            <Link to="/create">Создать заказ</Link>
          </nav>
        </header>

        <main className="main">
          <Routes>
            <Route path="/" element={<OrdersListPage />} />
            <Route path="/create" element={<CreateOrderPage />} />
            <Route path="/orders/:orderNumber" element={<OrderDetailsPage />} />
          </Routes>
        </main>
      </div>
  );
}

export default App;