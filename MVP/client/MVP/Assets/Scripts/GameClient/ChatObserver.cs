using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ChatObserver
{
    public void ReceiveMessage(GameClient.ChatMessage chatMessage);
}
