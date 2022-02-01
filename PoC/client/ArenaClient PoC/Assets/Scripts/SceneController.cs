using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
public class SceneController : MonoBehaviour
{
    private ColyseusClient client;
    private ColyseusRoom<State> room;
    // Start is called before the first frame update
    async void Start()
    {
        client = new ColyseusClient("ws://localhost:3000");
        room = await client.JoinOrCreate<State>("myRoom"/* , Dictionary of options */);
        await room.Send("action", new { message ="hello"});
        
    }

    // Update is called once per frame
    void Update()
    {
            
    }
}
