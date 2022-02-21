using UnityEngine;
using UnityEngine.UI;

public class SendHelloButton : MonoBehaviour
{
    public Button deconnexionButton;
    private GameClient gameClient;
    // Start is called before the first frame update
    void Start()
    {
        Button test = deconnexionButton.GetComponent<Button>();
        test.onClick.AddListener(sendHello);
        gameClient = findGameClient();
    }

    private GameClient findGameClient(){
        GameObject t = GameObject.Find("GameClient");
        return t.GetComponent<GameClient>();
    }

    async void sendHello()
    {
        await gameClient.room.Send("action", new {message= "hello"});
    }
}