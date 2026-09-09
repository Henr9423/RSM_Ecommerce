import axios from "axios"
import { useNavigate } from "react-router";
import { DeliveryOptions } from "./DeliveryOptions";

export function PaymentSummary({orderInfo, setOrderInfo, paymentSummary, loadCart, deliveryOptions, cart}) {
    const navigate=useNavigate();
    const createOrder=async ()=>{
      
        await axios.post('/api/Order',orderInfo);
        await loadCart();
        navigate('/order-login')
        
    }

    function handleChange(event) {
        const {name,value} = event.target;
        setOrderInfo(prev =>({
            ...prev,
            [name]:value
        }) )

    }
        


    if (!paymentSummary || !deliveryOptions) {
     return <p>Loading...</p>;
    }

    return (
        
        <div className="payment-summary">
            <h>Coupon Code: </h>
            <input 
                type="text" 
                name="couponCode" 
                value={orderInfo.couponCode} 
                onChange={handleChange} 
                placeholder="Coupon code">
            </input>

            <DeliveryOptions deliveryOptions={deliveryOptions} loadCart={loadCart} cart={cart} />
            <div className="payment-summary-title">
                Payment Summary
            </div>

            {paymentSummary && (
                <>
                    <div className="payment-summary-row"
                        data-testid='items-summary'>
                        <div>Items ({paymentSummary.itemsCount}):</div>
                        <div className="payment-summary-money">
                            {paymentSummary.productCost}
                        </div>
                    </div>

                    <div className="payment-summary-row"
                        data-testid='shipping-summary'>
                        <div>Shipping &amp; handling:</div>
                        <div className="payment-summary-money">{paymentSummary.shippingCost}</div>
                    </div>

                    <div className="payment-summary-row subtotal-row"
                        data-testid='subtotal-summary'>
                        <div>Total before tax:</div>
                        <div className="payment-summary-money">{paymentSummary.totalCostBeforeTax}</div>
                    </div>

                    <div className="payment-summary-row"
                        data-testid='tax-summary'>
                        <div>Estimated tax (10%):</div>
                        <div className="payment-summary-money">{paymentSummary.tax}</div>
                    </div>

                    <div className="payment-summary-row total-row"
                        data-testid='total-summary'>
                        <div>Order total:</div>
                        <div className="payment-summary-money">{paymentSummary.totalCost}</div>
                    </div>

                    <button className="place-order-button button-primary" 
                        data-testid='placeOrderButton'
                        onClick={createOrder}>
                        Place your order
                    </button>


                </>

            )}


        </div>

    )
}