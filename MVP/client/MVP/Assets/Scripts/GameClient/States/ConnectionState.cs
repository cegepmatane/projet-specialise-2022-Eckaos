// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.32
// 

using Colyseus.Schema;

public partial class ConnectionState : Schema {
	[Type(0, "array", typeof(ArraySchema<string>), "string")]
	public ArraySchema<string> players = new ArraySchema<string>();

	[Type(1, "array", typeof(ArraySchema<string>), "string")]
	public ArraySchema<string> spectators = new ArraySchema<string>();
}

