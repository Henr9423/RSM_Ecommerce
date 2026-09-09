
import axios from 'axios';
import { OrderSummary } from './OrderSummary';
import { PaymentSummary } from './PaymentSummary';
import { useState, useEffect } from 'react';
import { CheckoutHeader } from './CheckoutHeader';
import './CheckoutPage.css'
import { OrderInfoInput } from './OrderInfoInput';


export function CheckoutPage({ cart, loadCart }) {

    const [deliveryOptions, setDeliveryOptions] = useState([]);
    const [paymentSummary, setPaymentSummary] = useState(null);
    const [inputOrderInfo, setInputOrderInfo]=useState(false);
    const [orderInfo, setOrderInfo] = useState({
        firstName: "",
        lastName: "",
        email: "",
        addressLine1: "",
        addressLine2: "",
        city: "",
        stateOrRegion: "",
        postalCode: "",
        country: "",
        phoneNumber: "",

        billingSameAsShipping: true,

        billingAddressLine1: "",
        billingAddressLine2: "",
        billingCity: "",
        billingStateOrRegion: "",
        billingPostalCode: "",
        billingCountry: "",

        couponCode: ""
    
    });

    useEffect(() => {
        const fetchCheckoutData = async () => {
            const response = await axios.get('/api/DeliveryOptions')
            console.log(response.data);
            setDeliveryOptions(response.data)
        }

        fetchCheckoutData();
    }, [])

    useEffect(() => {
        const fetchPaymentSummary = async () => {
            const response = await axios.get('/api/PaymentSummary')

            setPaymentSummary(response.data)
        }
        fetchPaymentSummary();

    }, [cart])


    if(!cart ||cart.items.length===0)
    {
        return (
        <>
          <title>Checkout</title>
            <link rel="icon" type="image/svg+xml" href="/images/cart-favicon.png" />

            <CheckoutHeader cart={cart} />

            <div className="checkout-page">
                <div className="page-title">Review your order</div>
            
                <p>Your cart is empty.</p>
                
            </div>
        </>)
    
    }

    return (
        <>
            <title>Checkout</title>
            <link rel="icon" type="image/svg+xml" href="/images/cart-favicon.png" />


            <CheckoutHeader cart={cart} />

            <div className="checkout-page">
                <div className="page-title">Review your order</div>
             
                <div className="checkout-grid">
                    {!inputOrderInfo &&( 
                    <>
                        <OrderSummary cart={cart}  loadCart={loadCart} deliveryOptions={deliveryOptions} />
                        <button className="place-order-button button-primary" 
                            data-testid='continueToOrderInfoInput'
                            onClick={()=> setInputOrderInfo(true)}>
                            Continue
                        </button>
                    </>
                    )}
                   
                    {inputOrderInfo && (
                        <>
                            <OrderInfoInput orderInfo={orderInfo} setOrderInfo={setOrderInfo}></OrderInfoInput>
                            <PaymentSummary orderInfo={orderInfo} setOrderInfo={setOrderInfo}
                             paymentSummary={paymentSummary} 
                             loadCart={loadCart} 
                             deliveryOptions={deliveryOptions} 
                             cart={cart}
                            />
                        </>
                    )}
                   
                </div>
            </div>
        </>

    );
}