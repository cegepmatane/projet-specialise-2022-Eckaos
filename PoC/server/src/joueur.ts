import { Schema, type } from "@colyseus/schema";

export class Joueur extends Schema{
    @type("string") sessionId:string
}