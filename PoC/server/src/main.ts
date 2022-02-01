import { Server } from "colyseus";
import { MyRoom } from "./myRoom";
const port = parseInt(process.env.port, 10) || 3000

const gameServer = new Server()
gameServer.listen(port)
gameServer.define("myRoom", MyRoom)

console.log(`[GameServer] Listening on Port: ${port}`)