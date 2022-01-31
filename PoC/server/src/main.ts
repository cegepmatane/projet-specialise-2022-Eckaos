import { Server } from "colyseus";
const port = parseInt(process.env.port, 10) || 3000

const gameServer = new Server()
gameServer.listen(port)
console.log(`[GameServer] Listening on Port: ${port}`)
