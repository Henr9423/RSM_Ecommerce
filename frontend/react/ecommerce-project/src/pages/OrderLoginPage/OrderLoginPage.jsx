import { Header } from "../../components/Header";
import { EmailInput } from "./EmailInput";
import { useState } from "react";
import "./OrderLoginPage.css"
import { OrderNumberInput } from "./OrderNumberInput";
import { RequestGuestAccessButton } from "./RequestGuestAccessButton.jsx";
import { VerificationCodeInput } from "./VerificationCodeInput.jsx";
import { VerifyAccessCodeButton } from "./VerifyAccessCodeButton.jsx";

export function OrderLoginPage({cart}) {
    
     const [email, setEmail] = useState("");
     const [orderNumber,setOrderNumber]=useState("");
     const [isVerificationCodeSent, setIsVerificationCodeSent]=useState(false)
     const [verificationCode,setVerificationCode]=useState("")
  
    if(isVerificationCodeSent==false)
    {
        return (
            <div className="order-login">

                <Header cart={cart}></Header>

                <div className="login-container">
                    <EmailInput setEmail={setEmail}></EmailInput>
                    <OrderNumberInput setOrderNumber={setOrderNumber}></OrderNumberInput>
                    <RequestGuestAccessButton email={email} orderNumber={orderNumber} setIsVerificationCodeSent={setIsVerificationCodeSent}></RequestGuestAccessButton>
                </div>

            </div>

        ) 
    }
    else
    {
        return <>
         <div className="order-login">

                <Header cart={cart}></Header>

                <div className="login-container">
                    <VerificationCodeInput email={email} setVerificationCode={setVerificationCode}></VerificationCodeInput>
                    <VerifyAccessCodeButton verificationCode={verificationCode} orderNumber={orderNumber}></VerifyAccessCodeButton>
                </div>

            </div>
        </>
    }
}