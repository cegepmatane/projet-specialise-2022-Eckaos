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

    public string roomId;
    public int clientNumber;
    

    public void Initialization(string roomName, int clientNumber, ToggleGroup toggleGroup)
    {
        this.roomId = roomName;
        this.clientNumber = clientNumber;
        UpdateUI();
        toggle.group = toggleGroup;
        toggle.onValueChanged.AddListener(
            delegate 
            {
                if(toggle.isOn)
                    OnRoomItemClick();
            });
    }

    public void UpdateUI()
    {
        this.roomName.text = roomId;
        this.playerNumber.text = clientNumber+"/"+2;
    }
    public bool IsSelected() => toggle.isOn;
    public string GetName() => roomName.text;
    public void SetRoomId(string id) {
        this.roomId = id;
        joinButton.onClick.AddListener(onJoinButtonClick);
    }
    private void onJoinButtonClick()
    {
        GameClient.GetInstance().Join(roomId);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void OnRoomItemClick() => toggle.group.GetComponent<LobbyManager>().SetCurrentRoomId(roomId);
}
