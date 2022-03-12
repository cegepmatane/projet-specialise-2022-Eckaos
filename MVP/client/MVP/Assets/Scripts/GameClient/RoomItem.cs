using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Colyseus;
using UnityEngine.SceneManagement;

public class RoomItem : MonoBehaviour
{
    [SerializeField]
    private Text roomName;
    [SerializeField]
    private Text playerNumber;
    [SerializeField]
    public Button joinButton;
    [SerializeField]
    public Toggle toggle;

    public ColyseusRoomAvailable room;
    

    public void Initialization(string roomName, int playerNumber, ToggleGroup toggleGroup)
    {
        this.roomName.text = roomName;
        this.playerNumber.text = playerNumber+"/"+2;
        toggle.group = toggleGroup;
        toggle.onValueChanged.AddListener(
            delegate 
            {
                if(toggle.isOn)
                    OnRoomItemClick();
            });
    }
    public bool IsSelected() => toggle.isOn;
    public string GetName() => roomName.text;
    public void SetRoom(ColyseusRoomAvailable room) {
        this.room = room;
        joinButton.onClick.AddListener(onJoinButtonClick);
    }
    private void onJoinButtonClick()
    {
        GameClient.GetInstance().Join(room.roomId);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void OnRoomItemClick() => toggle.group.GetComponent<RoomManager>().SetCurrentRoom(room);
}
