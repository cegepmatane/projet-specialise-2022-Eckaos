using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChatManager : MonoBehaviour, ChatObserver
{
    public InputField chatInput;
    private GameClient client;
    public Chat chat;
    void Start()
    {
        client = GameClient.GetInstance();
        client.RegisterObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Return) && !chatInput.text.Equals(""))
        {
            client.SendChatMessage(chatInput.text);
            chatInput.text = "";
        }
    }

    public void ReceiveMessage(GameClient.ChatMessage chatMessage)
    {
        chat.AddMessage(chatMessage);
    }
}
