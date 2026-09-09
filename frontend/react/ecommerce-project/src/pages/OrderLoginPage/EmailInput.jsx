export function EmailInput({setEmail})
{

    return(
      <div className="input-group">
        <div>Email:</div>
        <input className="email-input" placeHolder="Email" onChange={(e)=>setEmail(e.target.value)}></input>
      </div>
    )
}