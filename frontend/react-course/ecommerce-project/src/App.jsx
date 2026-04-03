import axios from 'axios'
import { useState, useEffect } from 'react'
import { Routes, Route } from 'react-router'
import { HomePage } from './pages/home/HomePage'
import { CheckoutPage } from './pages/checkout/CheckoutPage'
import { OrdersPage } from './pages/orders/OrdersPage'
import './App.css'
import { TrackingPage } from './pages/tracking/TrackingPage'
import { NotFoundPage } from './pages/NotFoundPage/NotFoundPage'




function App() {
  const [cart, setCart] = useState([])
  const loadCart = async () => {
      const response = await axios.get('/api/cart-items?expand=product')
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
      <Route path='tracking/:orderId/:productId' element={<TrackingPage cart={cart} />} />
      <Route path="*" element={<NotFoundPage cart={cart} />} />
    </Routes>

  )
}

export default App
