import { useState} from 'react'
import {Chatbot} from "supersimpledev"
import loadingSpinnerGif from "../assets/loading-spinner.gif"
import './ChatInput.css'

export function ChatInput({chatMessages, setChatMessages}){
          const [inputText,setInputText]=useState('');
          const [isLoading,setIsLoading]=useState(false);

          function saveInputText(event){
            setInputText(event.target.value);

          }

          function handleClearMessagesButton()
          {
            setChatMessages([]);
            localStorage.setItem('messages',JSON.stringify([]));
          }

          async function sendMessage(){
            if(isLoading==false && inputText!="")
            {
              setIsLoading(true)
              const robotId=crypto.randomUUID();

              const newChatMessages=[...chatMessages ,
                {message: inputText, 
                sender: 'user',
                id: crypto.randomUUID()},
                {message: <img className="loading-spinner" src={loadingSpinnerGif} />, 
                sender: 'robot',
                id: robotId}
              ]

              setChatMessages(newChatMessages);

              const textToSend=inputText;
              setInputText('')

              const response=await Chatbot.getResponseAsync(textToSend);

              setChatMessages(prev => prev.map(m => m.id === robotId ? { ...m, message: response } : m ) );
              
              setIsLoading(false)
            }
      
          }

          function handleOnKeyDown(event)
          {
            if(event.key=='Enter')
            { 
              sendMessage();

            }

            if(event.key=="Escape")
            {
              setInputText("")
            }
          
          }

          return (
          <div className="chat-input-container">
            <input 
              placeholder="Send a message to Chatbot" 
              size="30"
              onChange={saveInputText}
              value={inputText}
              onKeyDown={handleOnKeyDown}
              className="chat-input"
            />
            <button 
            onClick={sendMessage}
            className="send-button"
             
            >Send</button>

            <button className='clear-button'  onClick={handleClearMessagesButton}>
              Clear
            </button>
          </div>
          );
        }
