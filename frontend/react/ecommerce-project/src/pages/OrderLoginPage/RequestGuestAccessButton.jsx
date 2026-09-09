import {useState }  from "react";
import axios from "axios";
export function RequestGuestAccessButton({email, orderNumber, setIsVerificationCodeSent})
{
   const [errorMessage, setErrorMessage] = useState("");
   const RequestGuestAccess = async () => {
  

    try {
      await axios.post('/api/Order/guest/request-access',{
        email:email,
        orderNumber:orderNumber
      })
      setIsVerificationCodeSent(true);

    } catch(error) {
                if (error.response) {
            // Backend responded with 4xx/5xx
            setErrorMessage(
            error.response.data?.message || "Unable to verify order."
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
                onClick={RequestGuestAccess}>
                Verify Order
            </button>

            {errorMessage && (<p>{errorMessage}</p>)}
        </>
    )


    
}