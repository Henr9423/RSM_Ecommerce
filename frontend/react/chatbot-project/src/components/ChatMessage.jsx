   
import robotProfileImage from '../assets/robot.png'
import myProfileImage from'../assets/Profil billede 2025.jpg'
import './ChatMessage.css';
import dayjs from 'dayjs';
import { useState } from 'react';
   
export function ChatMessage({message,sender}){
          // Different ways of doing the same thing. 
          // const message=props.message;
         // const sender= props.sender;
         // const {message, sender} =props;

         /*
         if (sender==='robot')
         {
           return (
            <div>
              <img src="robot.png" width="50"/>
              {message}
            </div>
          );
         }
         */ 


         const time =dayjs().valueOf();
         const [currentTime]=useState(dayjs(time).format('h:mma'))

          return (
            <div className={
              sender==="user"
              ?('chat-message-user')
              :('chat-message-robot')
            }>
              {sender=== 'robot' &&  (
                <img src={robotProfileImage} className="chat-message-profile"  />
              ) }
              <div className="chat-message-text">
                {message}
                <div className="time-on-message">
                    {currentTime}
                </div>
              </div>
              {sender=== 'user' && (
                <img src={myProfileImage} className="chat-message-profile" />
              )}
            </div>
          );
        }