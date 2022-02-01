import { Schema, type } from "@colyseus/schema";

export class State extends Schema {
    @type('int16')
    seat: number;
}