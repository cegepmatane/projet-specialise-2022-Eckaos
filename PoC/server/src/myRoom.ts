import { Room, Client } from "colyseus";
import { ConnexionState } from "./connexion_state";
import { Joueur } from "./joueur";
import { Spectateur } from "./spectateur";

export class MyRoom extends Room<ConnexionState> {
  onCreate (options: any) {
    this.setState(new ConnexionState())
    console.log("create")
    this.onMessage("action", (client, message) => {
      if(this.state.spectateurs.includes(client.sessionId)) return
      this.broadcast("action", message)
      console.log(client.sessionId, "sent 'action' message: ", message)
    });
  }

  onJoin (client: Client, options: any) {
    console.log("client "+ client.sessionId +" joined")
    this.attribuerRole(client)
    console.log("nombre de joueurs : " + this.state.joueurs.length, "nombre de spectateurs : " + this.state.spectateurs.length)
  }

  private attribuerRole(client:Client) {
    if(this.clients.length > 2)
      this.state.spectateurs.push(client.sessionId)
    else
      this.state.joueurs.push(client.sessionId)
  }

  onLeave (client: Client, consented: boolean) {
    this.retirerRole(client)
    console.log("nombre de joueurs : " + this.state.joueurs.length, "nombre de spectateurs : " + this.state.spectateurs.length)
  }

  private retirerRole(client:Client){ 
    if(this.state.spectateurs.includes(client.sessionId))
      this.state.spectateurs.splice(this.state.spectateurs.indexOf(client.sessionId), 1)
    if(this.state.joueurs.includes(client.sessionId))
      this.state.joueurs.splice(this.state.joueurs.indexOf(client.sessionId),1)
  }

  onDispose() {
  }

  
}
