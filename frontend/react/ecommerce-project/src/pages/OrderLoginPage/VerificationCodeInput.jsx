
export function VerificationCodeInput ({email, setVerificationCode}){


    return(
        <>
            <div className="input-group">
                <div>Input Verification code that was sent to {email} :</div>
                <input className="order-number-input" placeholder="VerificationCode" onChange={(e)=>setVerificationCode(e.target.value)}></input>
            </div>

          
        </>


    );

}