    import {useState} from "react"
    import "./PasswordInput.css";
    function PasswordInput()
    {
        const [showPassword,setShowPassword]=useState(true)
        
        function handleButtonClicked()
        {
            setShowPassword(!showPassword)
        }

        return(
            <div className="password-input-container">
                <input className="password-input" placeholder="Password" type={showPassword? "text":"password"} />
                <button className="hide-button" onClick={handleButtonClicked}>{showPassword?<>Hide</>: <>Show</>}</button>
            
            </div>
        )
    }
export default PasswordInput;