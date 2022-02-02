import { Schema, type } from "@colyseus/schema";

export class Spectateur extends Schema{
    @type("string") sessionId:string
}