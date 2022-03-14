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
      this.classes = this.getCharactersClass();
      this.broadcast("Start");
    })

    this.onMessage("Initialization", (client, message) => {
      client.send("MapInitialization", {"walls": this.walls});
      client.send("CharacterInitialization", {"positions":this.positions, "classNameList":this.classes, "idList": this.clients.map(c => c.sessionId).slice(0, 2)})
    });

    this.onMessage("End_Turn", (client, message) => {
      this.broadcast("End_Turn");
    });

    //TODO faire en sorte que les classes soit générer par le serveur

    this.onMessage("Action", (client, message) => {
      this.clients.forEach(c => {
        if(c.id !== client.id)
          c.send("Action", message)
      })
    });
  }

  onJoin (client: Client, options: any) {
    this.giveRole(client)
    if(this.clients.length === 1)
      client.send("ButtonStart");
  }

  private giveRole(client:Client) {
    if(this.clients.length > 2)
      this.state.spectators.push(client.sessionId)
    else
      this.state.players.push(client.sessionId)
  }

  onLeave (client: Client, consented: boolean) {
    this.removeRole(client)
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

  private getRandomClassName()
  {
    const values = Object.keys(Class);
    return values[Math.floor(Math.random() * values.length)];
  }

  private getCharactersClass()
  {
    var classNameList = [];
    for (let i = 0; i < 2; i++) {
      classNameList.push(this.getRandomClassName());
    }
    return classNameList;
  }
  
}

//TODO afficher le bouton start uniquement si on est le proprio de la room
//TODO STATE => players & spectators + creator


class Position {
  public x:number;
  public z:number;
  constructor(x: number, z:number) {
    this.x = x;
    this.z = z;
  }
}

enum Class{
  test="test",
  test2="test2"
}