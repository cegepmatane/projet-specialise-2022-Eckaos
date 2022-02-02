import { Schema, type, ArraySchema } from "@colyseus/schema";
import { Joueur } from "./joueur"
import { Spectateur } from "./spectateur";
export class ConnexionState extends Schema{
    @type(["string"]) joueurs = new ArraySchema<string>()
    @type(["string"]) spectateurs = new ArraySchema<string>()
}