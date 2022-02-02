using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
public class SceneController : MonoBehaviour
{
    private ColyseusClient client;
    private ColyseusRoom<ConnexionState> room;
    // Start is called before the first frame update

    class HelloMessage{
        public string message;
    }
    async void Start()
    {
        client = new ColyseusClient("ws://localhost:3000");
        room = await client.JoinOrCreate<ConnexionState>("myRoom"/* , Dictionary of options */);
        room.OnMessage<HelloMessage>("action", (message)=>{
            Debug.Log(message.message);
        });
        await room.Send("action", new { message ="hello"});
        
    }

    // Update is called once per frame
    async void Update()
    {
        if(Input.GetButtonDown("Jump"))
            await room.Send("action", new {message = "hello"});
    }

    private void OnApplicationQuit() {
        if(room != null) room.Leave();
    }
}
