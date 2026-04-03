import { Link, useParams} from "react-router";
import { Header } from "../../components/Header";
import './TrackingPage.css'
import { useEffect, useState } from "react";
import dayjs from "dayjs";
import axios from "axios";

export function TrackingPage({cart}) {

    const {orderId,productId}=useParams();
    const [order,setOrder]=useState(null)

    const [deliveryPercent,setDeliveryPercent]=useState(0)
 

    useEffect(()=>{
        const fetchOrder=async ()=>{
            const order= await axios.get(`/api/orders/${orderId}?expand=products`)

            setOrder(order.data)
            const product=order.data.products.find(product=>product.productId===productId);
            
            const totalDeliveryTimeMs=product.estimatedDeliveryTimeMs-order.data.orderTimeMs;
            const timePassedMs= dayjs().valueOf()-order.data.orderTimeMs;
    
            const deliveryPercentNotProcessed=  (timePassedMs/totalDeliveryTimeMs)*100;
            setDeliveryPercent(deliveryPercentNotProcessed>100 ? 100 : deliveryPercentNotProcessed);
            
        }
        
      fetchOrder();

    },[orderId,productId])
    
    
    
    const orderProduct = order?.products.find(
    (product) => String(product.productId) === String(productId)
  );

    const isPreparing=deliveryPercent<33;
    const isShipped=deliveryPercent>=33 && deliveryPercent<100;
    const isDelivered=deliveryPercent===100
    
    if(!order || !orderProduct) {return null}
    

    return (
      
        <>
            <title>Tracking</title>
            <link rel="icon" type="image/svg+xml" href="/images/tracking-favicon.png" />

            <Header cart={cart} />
            <div className="tracking-page">
                <div className="order-tracking">
                    <Link className="back-to-orders-link link-primary" to="/orders">
                        View all orders
                    </Link>

                    <div className="delivery-date">
                       {deliveryPercent>=100 ? <>Delivered on</> :<> Arriving on</>} {dayjs(orderProduct.estimatedDeliveryTimeMs).format('dddd, MMMM D')}
                    </div>

                    <div className="product-info">
                        {orderProduct.product.name}
                    </div>

                    <div className="product-info">
                        Quantity: {orderProduct.quantity}
                    </div>

                    <img className="product-image" src="images/products/athletic-cotton-socks-6-pairs.jpg" />

                    <div className="progress-labels-container">
                        <div className={`progress-label ${isPreparing && "current-status"}`}>
                            Preparing
                        </div>
                        <div className={`progress-label ${isShipped && "current-status"}`}>
                            Shipped
                        </div>
                        <div className={`progress-label ${isDelivered && "current-status"}`}>
                            Delivered
                        </div>
                    </div>

                    <div className="progress-bar-container">
                        <div className="progress-bar" style={{width:`${deliveryPercent}%`}}></div>
                    </div>
                </div>
            </div>

        </>

    );
}