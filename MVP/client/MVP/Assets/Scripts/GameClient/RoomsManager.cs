using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomsManager : MonoBehaviour, ClassPickerObserver, WaitingRoomObserver
{
    private GameClient client;
    private ColyseusRoom<ConnectionState> room;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button returnButton;
    [SerializeField]
    private Dropdown sizeDropdown;
    [SerializeField]
    private ClassPicker classPicker;

    private void Start() {
        client = GameClient.GetInstance();
        startButton.onClick.AddListener(StartGame);
        startButton.gameObject.SetActive(false);
        returnButton.onClick.AddListener(Return);
        classPicker.RegisterObserver(this);
        client.RegisterObserver(this);
    }

    private void Update() {
        if(room == null)
            room = client.GetCurrentRoom();
        if(client.CanStartGame())
            startButton.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        if(room == null) return;
        Size size = Size.GetSize(sizeDropdown.value);
        room.Send("Start", new {xSize = size.length, zSize = size.width, class1 = classPicker.GetClass1(), class2 = classPicker.GetClass2() });
    }

    public void Return()
    {
        room.Leave();
        client.ConnectToLobbyRoom();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ChangeClass1()
    {
        room?.Send("ChangeClass1", classPicker.GetClass1());
    }
    public void ChangeClass2()
    {
        room?.Send("ChangeClass2", classPicker.GetClass2());
    }
    public void ChangeClass1UI(string class1)
    {
        classPicker.SetClass1(class1);
    }
    public void ChangeClass2UI(string class2)
    {
        classPicker.SetClass2(class2);
    }


    class Size
    {

        public readonly int length;
        public readonly int width;

        public static Size Normal = new Size(15,15);
        public static Size Small = new Size(10,10);
        public static Size Large = new Size(20,20);

        private static Size[] sizes ={Size.Small, Size.Normal, Size.Large};

        public Size(int length, int width)
        {
            this.length = length;
            this.width = width;
        }

        public static Size GetSize(int code)
        {
            if(code >= sizes.Length || code < 0) return Size.Normal;
            return sizes[code];
        }
    }
}
