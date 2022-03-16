import { Room, Client } from "colyseus";
import { ConnectionState } from "./schema/ConnectionState";

export class GameRoom extends Room<ConnectionState> {
  private walls : boolean[][];
  private positions : Position[];
  private classes : string[];

  onCreate (options: any) {
    this.setState(new ConnectionState())
    this.onMessage("Start", (client, message) => {
      this.walls = this.generateMap(message.xSize, message.zSize)
      this.positions = this.generatePositions(this.walls);
      this.classes = [message.class1, message.class2];
      this.broadcast("Start");
    })

    this.onMessage("Initialization", (client, message) => {
      client.send("MapInitialization", {"walls": this.walls});
      client.send("CharacterInitialization", {"positions":this.positions, "classNameList":this.classes, "idList": this.clients.map(c => c.sessionId).slice(0, 2)})
    });

    this.onMessage("End_Turn", (client, message) => {
      this.broadcast("End_Turn");
    });

    this.onMessage("Action", (client, message) => {
      this.clients.forEach(c => {
        if(c.id !== client.id)
          c.send("Action", message)
      })
    });

    this.onMessage("SendMessage", (client, message) => this.broadcast("ReceiveMessage", message));
    this.onMessage("ChangeClass1", (client, message) => 
    {
      this.clients.forEach(c => {
        if(c.sessionId != client.sessionId)
          c.send("ChangeClass1", message);
      })
    });
    this.onMessage("ChangeClass2", (client, message) => 
    {
      this.clients.forEach(c => {
        if(c.sessionId != client.sessionId)
          c.send("ChangeClass2", message);
      })
    });
  }

  onJoin (client: Client, options: any) {
    this.giveRole(client)
    client.userData = {pseudo : options.pseudo}
    if(this.clients.length === 1)
      client.send("ButtonStart");
    this.broadcast("Players", this.clients.map(c => {return c.userData.pseudo}).slice(0,2))
    this.broadcast("Spectators", this.clients.map(c => {return c.userData.pseudo}).slice(2))
  }

  private giveRole(client:Client) {
    if(this.clients.length > 2)
      this.state.spectators.push(client.sessionId)
    else
      this.state.players.push(client.sessionId)
  }

  onLeave (client: Client, consented: boolean) {
    this.removeRole(client)
    if(this.state.players.includes(client.sessionId))
      this.broadcast("RemovePlayer", client.userData.pseudo);
    else
      this.broadcast("RemoveSpectator", client.userData.pseudo)
    if(this.clients.length === 0) this.disconnect();
  }

  private removeRole(client:Client){ 
    if(this.state.spectators.includes(client.sessionId))
      this.state.spectators.splice(this.state.spectators.indexOf(client.sessionId), 1)
    if(this.state.players.includes(client.sessionId))
      this.state.players.splice(this.state.players.indexOf(client.sessionId),1)
  }

  onDispose() {
  }


  private generateMap(xSize : number, zSize : number) : boolean[][] {
    let walls : boolean[][] = this.initWalls(xSize, zSize);
    let min : number = Math.log2(xSize*zSize);
    let max : number = xSize <= zSize ? xSize : zSize;
    let numberOfWalls : number = Math.floor(Math.random() * (max-min) + min);
    for (let x = 0; x < numberOfWalls; x++) 
      this.generateWalls(walls, xSize, zSize);
    return walls;
  }

  private initWalls(xSize :number, zSize : number) : boolean[][] {
    let walls : boolean[][] = [];
    for (let x = 0; x < xSize; x++){
      walls[x] = [];
      for (let z = 0; z < zSize; z++)
        walls[x][z] = false;
    }
    
    return walls;
  }

  private generateWalls(walls : boolean[][],xSize : number, zSize : number) {
    let x : number;
    let z : number;
    do {
      x = Math.floor(Math.random() * xSize);
      z = Math.floor(Math.random() * zSize);
    } while (walls[x][z]);
    walls[x][z] = true;
  }

  private generatePositions(walls : boolean[][]) : Position[]
  {
    let positions : Position[] = [];
    for (let i = 0; i < 2; i++) {
      
      let x : number;
      let z : number;
      do {
        x = Math.floor(Math.random() * walls.length);
        z = Math.floor(Math.random() * walls[x].length);
      } while (walls[x][z] || positions.includes(new Position(x,z)));
      positions.push(new Position(x,z));
    }
    return positions
  }
  
}

class Position {
  public x:number;
  public z:number;
  constructor(x: number, z:number) {
    this.x = x;
    this.z = z;
  }
}