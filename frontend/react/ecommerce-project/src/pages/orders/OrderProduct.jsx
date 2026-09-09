import axios from "axios"
import dayjs from "dayjs"
import buyAgain from "../../assets/images/icons/buy-again.png"
import { Link } from "react-router"

export function OrderProduct({orderProduct,order, loadCart}) {

     const addToCart=async ()=>{
        await axios.post('/api/Cart/items',
            {
                productVariantId: orderProduct.product.variantId,
                quantity: 1
            }
        )

        await loadCart()

        

    }

    return (
        <>
            <div className="product-image-container">
                <img src={orderProduct.product.imageUrl} />
            </div>

            <div className="product-details">
                <div className="product-name">
                    {orderProduct.product.name}
                </div>
                <div className="product-delivery-date">
                    Arriving on: {dayjs.utc(orderProduct.estimatedDeliveryTime).format('MMMM D, YYYY')}
                </div>
                <div className="product-quantity">
                    Quantity: {orderProduct.quantity}
                </div>
                <button className="buy-again-button button-primary" onClick={addToCart}>
                    <img className="buy-again-icon" src={buyAgain} />
                    <span className="buy-again-message">Add to Cart</span>
                </button>
            </div>

            <div className="product-actions">
                <Link to={`/tracking/${order.id}/${orderProduct.productId}`}>
                    <button className="track-package-button button-secondary">
                        Track package
                    </button>
                </Link>
            </div>

        </>

    )
}