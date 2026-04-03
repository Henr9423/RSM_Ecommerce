import { useState, useEffect} from 'react'
import {Chatbot} from 'supersimpledev'
import { ChatInput } from './components/ChatInput'
import ChatMessages from './components/ChatMessages'


import './App.css'


  function App(){
        const [chatMessages,setChatMessages]=useState(JSON.parse(localStorage.getItem('messages'))||[])
         /* [{
            message: 'hello Chatbot', 
            sender: 'user',
            id:'id1'
          },{
            
            message: 'Hello! How can i help you', 
            sender: 'robot',
            id:'id2'

          },{
            
            message: 'can you get me todays date?', 
            sender: 'user',
            id:'id3'
          },{
            
            message: 'Today is February 16', 
            sender: 'robot',
            id:'id4'
          }]);
         */
          useEffect(()=>{
            Chatbot.addResponses({"how are you doing?":"im doing fine thanks", "Never gonna": "give you up" })

          },[])

          useEffect(()=>{
            localStorage.setItem('messages',JSON.stringify(chatMessages));

          },[chatMessages])




          return (
            <div className="app-container">

            
              <ChatMessages chatMessages={chatMessages}/>
              
              <ChatInput chatMessages={chatMessages} setChatMessages={setChatMessages} />


            </div>

          );
        }

export default App
