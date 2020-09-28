import React from 'react';
import './App.css';
import axios from 'axios';


const baseURL = process.env.NODE_ENV === "development"
  ? "https://devhealthapi/"
  : "https://healthapi/"  //devhealthapi can be localhost with port, e.g. localhost:70707 
class App extends React.Component{
  constructor(props){
    super(props);
    this.userCredential={
      logonid:React.createRef(),
      passcode:React.createRef()
    }
    //This is needed to enable sending CORS cookie (say authenticated) to origin back with subsequent AJAX call
    this.app = axios.create({
      baseURL,
      withCredentials: true
  });
  this.state={
    authenticated:false,
    summary:''
  };
  }
  
  handleLogin=()=>{
    let loginData = {
      'LoginId':this.userCredential.logonid.current.value,
      'Passcode':this.userCredential.passcode.current.value
    };

   this.app.post("account/UserAccount/signin"
      ,loginData
      ,{'Content-Type': 'application/json' }
     )
    .then((res)=>{
      if(res.data==="Authenticated"){
        this.setState({
          authenticated:true,
          summary:''
        });
        console.log(res.data);
      }
    });
    
  }
  
  signOutApi=()=>{
    this.app.post("account/UserAccount/signout")
    .then((res)=>{
      console.log(res.data); 
      this.setState({
        authenticated:false,
        summary:''
      });
    });
  }

  handleApi=()=>{
    this.app.get("Patient/Visit/summary")
    .then((res)=>{
      console.log(res.data);
      this.setState({
        authenticated:true,
        summary: JSON.stringify(res.data,null,2)
      });
    });
  }
  render(){
  return (
    <div className="App">
      <div style={this.state.authenticated?{display:"none"}:{}}>
        <div> Email Id : <input type="email" ref={this.userCredential.logonid} defaultValue="hp@olto.dev" /></div>
        <div> Passcode : <input type='text' ref={this.userCredential.passcode} defaultValue="1234567"/></div>
        <div><button  onClick={()=>{this.handleLogin()}}>Login</button></div>
      </div>
      <div style={this.state.authenticated?{}:{display:"none"}}>
      <div><button  onClick={()=>{this.handleApi()}}>Visit Summary</button></div>
      <div><textarea rows={8} cols={50}  defaultValue={this.state.summary}></textarea ></div>
      <div><button  onClick={()=>{this.signOutApi()}}>Goodbye!</button></div>
      </div>
     </div>
  );
}
}

export default App;
