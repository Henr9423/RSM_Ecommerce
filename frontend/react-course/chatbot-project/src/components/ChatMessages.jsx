
import { useAutoScroll } from "./useAutoScoll";
import { ChatMessage } from './ChatMessage';
import './ChatMessages.css';

export function ChatMessages({chatMessages}) {
          

          const chatMessagesRef=useAutoScroll(chatMessages)
          
          return(
            <div className="chat-messages-container" ref={chatMessagesRef}>
             
              {chatMessages.length===0? <p>Welcome to the chatbot project! Send a message using the textbox below.</p>: <></>}
              {chatMessages.map((chatMessage)=> {
              return (
                <ChatMessage 
                message={chatMessage.message} 
                sender={chatMessage.sender} 
                key={chatMessage.id}
                />
              );
              })}
            </div>
          )

        }

        export default ChatMessages;