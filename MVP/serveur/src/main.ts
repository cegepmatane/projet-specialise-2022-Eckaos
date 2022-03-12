import { Server } from "colyseus";
import { GameRoom } from "./rooms/GameRoom";
const port = parseInt(process.env.port, 10) || 3000

const gameServer = new Server()
gameServer.listen(port)
gameServer.define("GameRoom", GameRoom).enableRealtimeListing();

console.log(`[GameServer] Listening on Port: ${port}`)