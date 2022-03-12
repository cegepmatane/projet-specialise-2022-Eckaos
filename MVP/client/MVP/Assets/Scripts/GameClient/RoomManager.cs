using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using Colyseus;

public class RoomManager : MonoBehaviour
{

    private GameClient client;
    public GameObject roomItemPrefab;
    public RectTransform parentTransform;
    private List<RoomItem> roomItems;
    private ToggleGroup toggleGroup;
    private ColyseusRoomAvailable currentRoom;

    private string[] scenePaths;
    private void Start() {
        roomItems = new List<RoomItem>();
        toggleGroup = gameObject.AddComponent<ToggleGroup>();
        toggleGroup.allowSwitchOff = false;
        client = GameClient.GetInstance();
    }
    private void Update() {
        client.AvailableRooms();
        foreach (var room in client.GetAvailableRooms())
        {
            if(roomItems.Select(roomItem => roomItem.room.roomId).Contains(room.roomId)) return;
            RoomItem roomItem = Instantiate(roomItemPrefab, Vector3.zero, Quaternion.identity).GetComponent<RoomItem>();
            SetUp(roomItem, room);
            roomItems.Add(roomItem);
        }
    }
    private void SetUp(RoomItem roomItem, ColyseusRoomAvailable room)
    {
        roomItem.Initialization(room.roomId, room.clients <= 2 ? ((int)room.clients) : 2, toggleGroup);
        roomItem.transform.SetParent(parentTransform);
        roomItem.transform.localScale = new Vector3(1,1,1);
        roomItem.transform.position = Vector3.zero;
        roomItem.SetRoom(room);
    }

    public void SetCurrentRoom(ColyseusRoomAvailable room) => currentRoom = room;
    public void CreateRoom()
    {
        client.Create();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void JoinRoom()
    {
        client.Join(currentRoom.roomId);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
}
