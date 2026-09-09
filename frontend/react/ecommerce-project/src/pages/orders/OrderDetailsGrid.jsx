
import { OrderProduct } from "./OrderProduct"

export function OrderDetailsGrid({ order, loadCart }) {
    
   
  
   
    return (
        <div className="order-details-grid">

            {order.orderItems.map((orderProduct) => {

                return (
                    <OrderProduct key={orderProduct.productId} orderProduct={orderProduct} order={order} loadCart={loadCart} />
                )

            })}


        </div>

    )
}