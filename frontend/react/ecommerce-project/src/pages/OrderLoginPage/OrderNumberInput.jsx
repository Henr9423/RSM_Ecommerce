export function OrderNumberInput ({setOrderNumber}){

    return(
        <>
            <div className="input-group">
                <div>Order number:</div>
                <input className="order-number-input" placeholder="order number" onChange={(e)=> setOrderNumber(e.target.value)}></input>
            </div>
        </>


    );

}