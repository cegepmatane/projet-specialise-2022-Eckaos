import { Schema, type, ArraySchema } from "@colyseus/schema";

export class ConnexionState extends Schema{
    @type(["string"]) joueurs = new ArraySchema<string>()
    @type(["string"]) spectateurs = new ArraySchema<string>()
}