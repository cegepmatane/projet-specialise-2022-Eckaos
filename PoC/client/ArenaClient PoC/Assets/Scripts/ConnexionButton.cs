using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnexionButton : MonoBehaviour
{
    public Button connexionButton;
    private GameClient gameClient;

    class HelloMessage{
        public string message;
    }
    void Start()
    {
        Button test = connexionButton.GetComponent<Button>();
        test.onClick.AddListener(creerRoom);
        gameClient = findGameClient();
    }

    private GameClient findGameClient(){
        GameObject t = GameObject.Find("GameClient");
        return t.GetComponent<GameClient>();
    }

    async void creerRoom()
    {
        if(gameClient == null)
            gameClient = findGameClient();
        gameClient.room = await gameClient.client.JoinOrCreate<ConnexionState>("myRoom");
        initialiserRoom();
    }

    private void initialiserRoom(){
        gameClient.room.OnMessage<HelloMessage>("action", (message)=>{
            Debug.Log(message.message);
        });
    }
}
