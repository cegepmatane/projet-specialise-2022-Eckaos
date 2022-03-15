using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using Colyseus;

public class LobbyManager : MonoBehaviour, LobbyObserver
{

    private GameClient client;
    public GameObject roomItemPrefab;
    public RectTransform parentTransform;
    private List<RoomItem> roomItems;
    private ToggleGroup toggleGroup;
    private string currentRoomId;
    public InputField pseudoInput;

    private void Start() {
        roomItems = new List<RoomItem>();
        toggleGroup = gameObject.AddComponent<ToggleGroup>();
        toggleGroup.allowSwitchOff = false;
        client = GameClient.GetInstance();
        client.RegisterObserver(this);
    }

    public void Start(List<(string roomId, int clientNumber)> roomList)
    {
        foreach (var room in roomList)
        {
            RoomItem roomItem = Instantiate(roomItemPrefab, Vector3.zero, Quaternion.identity).GetComponent<RoomItem>();
            SetUp(roomItem, room.roomId, room.clientNumber);
            roomItems.Add(roomItem);
        }
    }

    public void Add(string roomId, int clientNumber)
    {
        if(clientNumber == 0) return;RoomItem roomItem = null;
        if(roomItems.Exists(item => item.roomId.Equals(roomId)))
        {
            roomItem = roomItems.First(item => item.roomId.Equals(roomId));
            roomItem.clientNumber = clientNumber;
            roomItem.UpdateUI();
            return;
        }
        roomItem = Instantiate(roomItemPrefab, Vector3.zero, Quaternion.identity).GetComponent<RoomItem>();SetUp(roomItem, roomId, clientNumber);
        roomItems.Add(roomItem);
    }

    public void Remove(string roomId)
    {
        int i = roomItems.FindIndex(0, item => item.roomId.Equals(roomId));
        if(i == -1) return;
        RoomItem r = roomItems[i];
        roomItems.Remove(r);
        DestroyImmediate(r.gameObject);
    }

    private void SetUp(RoomItem roomItem, string roomId, int clientNumber)
    {
        roomItem.Initialization(roomId, clientNumber, pseudoInput,toggleGroup);
        roomItem.transform.SetParent(parentTransform);
        roomItem.transform.localScale = new Vector3(1,1,1);
        roomItem.transform.position = Vector3.zero;
        roomItem.SetRoomId(roomId);
    }

    public void SetCurrentRoomId(string roomId) => currentRoomId = roomId;
    public void CreateRoom()
    {
        if(pseudoInput.text.Equals(""))
            return; //TODO mettre un message d'erreur
        client.SetPseudo(pseudoInput.text);
        client.Create();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void JoinRoom()
    {
        if(pseudoInput.text.Equals(""))
            return; //TODO mettre un message d'erreur
        client.SetPseudo(pseudoInput.text);
        client.Join(currentRoomId);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
}
