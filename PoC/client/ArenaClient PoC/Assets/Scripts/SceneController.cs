using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
public class SceneController : MonoBehaviour
{
    private ColyseusClient client;
    private ColyseusRoom<State> room;
    // Start is called before the first frame update

    class HelloMessage{
        public string message;
    }
    async void Start()
    {
        client = new ColyseusClient("ws://localhost:3000");
        room = await client.JoinOrCreate<State>("myRoom"/* , Dictionary of options */);
        room.OnMessage<HelloMessage>("action", (message)=>{
            Debug.Log(message.message);
        });
        await room.Send("action", new { message ="hello"});
        
    }

    // Update is called once per frame
    void Update()
    {
            
    }
}
