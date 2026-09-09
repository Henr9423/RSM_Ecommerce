import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import timezone from "dayjs/plugin/timezone";

dayjs.extend(utc);
dayjs.extend(timezone);
export function DeliveryDate({ deliveryOptions, cart }) {

 
        const selectedDeliveryOption = deliveryOptions.find((deliveryOption) => {

            return deliveryOption.id === cart.deliveryOptionId;
        })


        return (
            <>
                <div className="delivery-date">
                    Delivery date: {dayjs(selectedDeliveryOption.estimatedDeliveryFrom).local().format('dddd, MMMM D')} - {dayjs(selectedDeliveryOption.estimatedDeliveryTo).local().format('dddd, MMMM D')} 
                </div>

            </>

        )
    ;


}