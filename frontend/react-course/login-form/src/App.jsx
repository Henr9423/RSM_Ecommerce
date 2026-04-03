import EmailInput from '../Components/EmailInput';
import PasswordInput from '../Components/PasswordInput';
import LoginButton  from '../Components/LoginButton';
import SignUpButton from '../Components/SignupButton';
import './App.css'

 function App(){
                    
                    


  return (
    <div className="app-container">
      <div className="form">
          <EmailInput />
          <PasswordInput />

          <div className="log-signup-button-container">
            <LoginButton />
            <SignUpButton />
          </div>

      </div>
    </div>

  );
}

export default App
