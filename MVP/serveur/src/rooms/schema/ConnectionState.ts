import { Schema, type, ArraySchema } from "@colyseus/schema";

export class ConnectionState extends Schema{
    @type(["string"]) players = new ArraySchema<string>();
    @type(["string"]) spectators = new ArraySchema<string>();
}