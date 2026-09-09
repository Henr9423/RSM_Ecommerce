import axios from "axios"
import './CartItemDetails.css'
import { useState } from "react"

export function CartItemDetails({cartItem, loadCart}) {
    const [isUpdating,setIsUpdating]=useState(false)
    const [quantity, setQuantity]=useState(cartItem.quantity)

    const deleteCartItem= async()=>{
        await axios.delete(`/api/Cart/Items/${cartItem.id}`);
        await loadCart();
    }
    
    const updateQuantity=async()=>{

        if(isUpdating===true)
        {
            await axios.put(`/api/Cart/Items/${cartItem.id}/quantity`,
                {
                    quantity: Number(quantity)
                }
            )

            await loadCart();
        }

        setIsUpdating(!isUpdating)

    }

    const onKeyDownQuantity=(event)=>{
        if(event.key==="Enter")
        {
            updateQuantity()
        }

        if(event.key==="Escape")
        {
            setQuantity(cartItem.quantity)
            setIsUpdating(false)
        }
    }

    return (
        <>
            <img className="product-image"
                src={cartItem.imageUrl} />

            <div className="cart-item-details">
                <div className="product-name">
                    {cartItem.productName}
                </div>
                <div className="product-price">
                    {cartItem.unitPrice}
                </div>
                <div className="product-quantity">
                    <span>
                        Quantity: {isUpdating?<input className="quantity-textbox" type="text" 
                            value={quantity} 
                            onChange={(event)=>{setQuantity(event.target.value)}}
                            onKeyDown={onKeyDownQuantity} />:<></>} <span className="quantity-label">{cartItem.quantity}</span>
                    </span>
                    <span className="update-quantity-link link-primary" 
                        onClick={updateQuantity}>
                        Update
                    </span>
                    <span className="delete-quantity-link link-primary" 
                    onClick={deleteCartItem}>
                        Delete
                    </span>
                </div>
            </div>
        </>

    )
}