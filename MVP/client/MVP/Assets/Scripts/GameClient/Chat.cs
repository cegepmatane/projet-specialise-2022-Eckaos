using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour
{
    public GameObject content;
    public GameObject messagePrefab;

    public void AddMessage(GameClient.ChatMessage chatMessage)
    {
        ChatMessageObject chatMessageObject = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity).GetComponent<ChatMessageObject>();
        chatMessageObject.transform.SetParent(content.transform);
        chatMessageObject.transform.localPosition = Vector3.zero;
        chatMessageObject.transform.localScale = new Vector3(1,1,1);
        chatMessageObject.message.text = chatMessage.pseudo+" : "+chatMessage.message;
    }
}
