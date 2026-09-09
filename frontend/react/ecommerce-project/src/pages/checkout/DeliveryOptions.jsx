import axios from "axios";
import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import timezone from "dayjs/plugin/timezone";

dayjs.extend(utc);
dayjs.extend(timezone);
import { formatMoney } from "../../utils/money";

export function DeliveryOptions({deliveryOptions, loadCart, cart}) {
    return (


        <div className="delivery-options">
            <div className="delivery-options-title">
                Choose a delivery option:
            </div>
            {deliveryOptions.map((deliveryOption) => {

                let priceString = 'FREE Shipping';
                if (deliveryOption.price > 0) {
                    priceString = `${deliveryOption.price}- Shipping`
                }
                const updateDeliveryOption=async ()=>{
                    await axios.put(`/api/Cart/items/delivery-option`,
                        {
                            deliveryOptionId: deliveryOption.id
                        }
                    );

                    await loadCart();
                }

                return (
                    <div key={deliveryOption.id} className="delivery-option" onClick={updateDeliveryOption}>
                        <input type="radio" 
                            checked={deliveryOption.id==cart.deliveryOptionId}
                            onChange={()=>{}}
                            className="delivery-option-input"
                            name={`delivery-option-${deliveryOption.id}`} />
                        <div>
                            <div className="delivery-option-date">
                                {dayjs(deliveryOption.estimatedDeliveryFrom).local().format('dddd, MMMM D')} - {dayjs(deliveryOption.estimatedDeliveryTo).local().format('dddd, MMMM D')} 
                            </div>
                            <div className="delivery-option-price">
                                {priceString}
                            </div>
                        </div>
                    </div>
                )

            })}

        </div>
    )
}