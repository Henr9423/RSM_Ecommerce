import axios from 'axios'
import { useState, useEffect } from 'react'
import { Routes, Route } from 'react-router'
import { HomePage } from './pages/home/HomePage'
import { CheckoutPage } from './pages/checkout/CheckoutPage'
import { OrdersPage } from './pages/orders/OrdersPage'
import './App.css'
import { TrackingPage } from './pages/tracking/TrackingPage'
import { NotFoundPage } from './pages/NotFoundPage/NotFoundPage'
import { OrderLoginPage } from './pages/OrderLoginPage/OrderLoginPage'




function App() {
  const [cart, setCart] = useState({
    status: "active",
    items: [],
    totalPrice: 0
  });
  const loadCart = async () => {
      const response = await axios.get('/api/Cart')
      setCart(response.data)

    }

  useEffect(() => {
    window.axios=axios;

    loadCart();

  }, [])

  return (
    <Routes>
      <Route index element={<HomePage cart={cart} loadCart={loadCart} />} />
      <Route path='checkout' element={<CheckoutPage cart={cart} loadCart={loadCart} />} />
      <Route path='orders' element={<OrdersPage cart={cart} loadCart={loadCart} />} />
      <Route path='orders/:orderNumber' element={<OrdersPage cart={cart} loadCart={loadCart} />} />
      <Route path='tracking/:orderId/:productId' element={<TrackingPage cart={cart} />} />
      <Route path="order-login" element={<OrderLoginPage cart={cart}></OrderLoginPage>}></Route>
      <Route path="*" element={<NotFoundPage cart={cart} />} />
      
    </Routes>

  )
}

export default App
