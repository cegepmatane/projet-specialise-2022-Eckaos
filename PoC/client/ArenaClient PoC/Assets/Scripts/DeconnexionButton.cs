using UnityEngine;
using UnityEngine.UI;

public class DeconnexionButton : MonoBehaviour
{
    public Button deconnexionButton;
    private GameClient gameClient;
    // Start is called before the first frame update
    void Start()
    {
        Button test = deconnexionButton.GetComponent<Button>();
        test.onClick.AddListener(leaveRoom);
        gameClient = findGameClient();
    }

    private GameClient findGameClient(){
        GameObject t = GameObject.Find("GameClient");
        return t.GetComponent<GameClient>();
    }

    void leaveRoom()
    {
        if(gameClient == null) 
            gameClient = findGameClient();
        if(gameClient.room == null) 
            return;
        gameClient.room.Leave();
    }
}