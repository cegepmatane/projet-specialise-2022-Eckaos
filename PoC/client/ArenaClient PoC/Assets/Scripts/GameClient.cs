using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
public class GameClient : MonoBehaviour
{
    public ColyseusClient client;
    public ColyseusRoom<ConnexionState> room;
    // Start is called before the first frame update

    class HelloMessage{
        public string message;
    }
    void Start()
    {
        client = new ColyseusClient("ws://localhost:3000");
    }
    
    private void OnApplicationQuit() {
        if(room != null) room.Leave();
    }

    public async void SendHello()
    {
        await gameClient.room.Send("action", new {message= "hello"});
    }
}
