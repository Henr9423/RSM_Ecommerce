    import "./LoginButton.css";
    
    function LoginButton()
    {
       
        
        function handleButtonClicked()
        {
        }

        return(
            <div className="login-button-container">
                <button onClick={handleButtonClicked} className="login-button">Login</button>
            
            </div>
        )
    }

export default LoginButton;