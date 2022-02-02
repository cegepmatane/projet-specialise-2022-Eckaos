import { Room, Client } from "colyseus";
import { State } from "./state";

export class MyRoom extends Room<State> {
  onCreate (options: any) {
    this.setState(new State())
    console.log("create")
    this.onMessage("action", (client, message) => {
      console.log(client.sessionId, "sent 'action' message: ", message);
      this.clients.forEach(c => {
        if(c.sessionId !== client.sessionId) {
          console.log(message.message)
          var m:string = message.message + " from "+ client.sessionId
          c.send("action", {message:m})
        }
      });
    });
  }

  onJoin (client: Client, options: any) {
    console.log("client "+ client.sessionId +" joined")
  }

  onLeave (client: Client, consented: boolean) {
  }

  onDispose() {
  }
}