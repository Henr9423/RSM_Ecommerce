
import { CartItemDetails } from "./CartItemDetails";
import { DeliveryOptions } from "./DeliveryOptions";
import { DeliveryDate } from "./DeliveryDate";

export function OrderSummary({deliveryOptions,cart, loadCart}) {

    if (!deliveryOptions) {
     return <p>Loading...</p>;
    }
    else if(deliveryOptions.length>0)
    {

        return (
            <div className="order-summary">
                <DeliveryDate cart={cart} deliveryOptions={deliveryOptions}></DeliveryDate>
                {deliveryOptions.length > 0 && cart.items.map((cartItem) => {
                

                    return (
                        <div key={cartItem.productId} className="cart-item-container">
                        

                            <div className="cart-item-details-grid">
                                
                                <CartItemDetails cartItem={cartItem} loadCart={loadCart} />
                            
                            </div>
                        </div>
                    );
                })}

            </div>
        )
    }
}