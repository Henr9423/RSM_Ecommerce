import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router";
export function VerifyAccessCodeButton({verificationCode, orderNumber})
{
    const [errorMessage, setErrorMessage] = useState("");
    const navigate=useNavigate();

    const VerifyAccess = async () => {
        try { 
            const response= await axios.post('/api/Order/guest/verify',{
                orderNumber:orderNumber,
                code:verificationCode
            })

            localStorage.setItem("guestToken",response.data.guestToken)
            navigate(`/orders/${orderNumber}`)

        } catch(error) {
        if (error.response) {
                // Backend responded with 4xx/5xx
                setErrorMessage(
                error.response.data?.message || "Unable to verify AccessCode."
                );
            } else {
                // Network error / server unreachable
                setErrorMessage("Could not connect to the server.");
            }
            
        }
    }
    return (
        <>
        
           <button className="verify-guest-button"
                data-testid="verify-guest-button"
                onClick={VerifyAccess}>
                Verify AccessCode
            </button>

            {errorMessage && (<p>{errorMessage}</p>)}
        </>
    )
}