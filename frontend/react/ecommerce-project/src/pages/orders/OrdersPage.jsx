import { OrdersGrid } from './OrdersGrid';
import { useState, useEffect } from 'react';

import { Header } from '../../components/Header';
import './OrdersPage.css'
import { api } from '../../api/api';
import { useParams } from 'react-router';

export function OrdersPage({ cart, loadCart }) {

    const [orders, setOrders] = useState([])
    const { orderNumber } = useParams();

    const isGuestOrderView = !!orderNumber;

    useEffect(() => {
        const fetchOrderData = async () => {
           const response=await api.get(`/Order/guest/${orderNumber}`)
               
            setOrders([response.data])
          
        }
        fetchOrderData();
       
    }, [orderNumber])

    return (
        <>
            <title>Orders</title>
            <link rel="icon" type="image/svg+xml" href="/images/orders-favicon.png" />



            <Header cart={cart} loadCart={loadCart} />

            <div className="orders-page">
                <div className="page-title">Your Orders</div>

                <OrdersGrid orders={orders} loadCart={loadCart} />


            </div>

        </>

    );
}