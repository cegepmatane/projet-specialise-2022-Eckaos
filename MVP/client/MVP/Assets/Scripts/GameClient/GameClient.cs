using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using System.Linq;
using System;
using UnityEngine.SceneManagement;


public class GameClient : IGameClient
{
    private static GameClient gameClient;
    private  ColyseusClient colyseusClient;
    private ColyseusRoom<ConnectionState> room;
    private bool canStartGame = false;
    private ClientObserver clientObserver;
    private ColyseusRoom<dynamic> lobby;
    private List<(string roomId, int clientNumber)> availableRoomList;
    private LobbyObserver lobbyObserver;
    

    public static GameClient GetInstance()
    {
        if(gameClient == null) gameClient = new GameClient();
        return gameClient;
    }

    private GameClient()
    {
        colyseusClient = new ColyseusClient("ws://localhost:3000");
        availableRoomList = new List<(string roomId, int clientNumber)>();
        ConnectToLobbyRoom();
    }

    public async void ConnectToLobbyRoom()
    {
        lobby = await colyseusClient.JoinOrCreate<dynamic>("Lobby");
        lobby.OnMessage<RoomListingData[]>("rooms", (data) => lobbyObserver.Start(data.Select(d => (d.roomId, d.clients)).ToList()));
        lobby.OnMessage<RoomListingData>("+", (data) => lobbyObserver.Add(data.roomId, data.clients));
        lobby.OnMessage<string>("-", (roomId) => lobbyObserver.Remove(roomId));
    }
    class RoomListingData
    {
        public int clients;
        public bool locked;
        public bool p;
        public double maxClients;
        public object metadata;
        public string name;
        public string processId;
        public string roomId;
        public bool unlisted;
    }
    public async void Create()
    {
        room = await colyseusClient.Create<ConnectionState>("GameRoom");//Change
        SetRoomCallback(room);
        await lobby.Leave();
    }
    public async void Join(string roomId)
    {
        room = await colyseusClient.JoinById<ConnectionState>(roomId);
        SetRoomCallback(room);
        await lobby.Leave();
    }

    private void SetRoomCallback(ColyseusRoom<ConnectionState> room)
    {
        room.OnMessage<NullReferenceException>("ButtonStart", (message) => canStartGame = true);
        room.OnMessage<NullReferenceException>("Start", (message) => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        room.OnMessage<Walls>("MapInitialization", (message) => clientObserver.InitializeMap(message.GetWalls()));
        room.OnMessage<CharactersInitialization>("CharacterInitialization", (message) => clientObserver.InitializeCharacters(message.GetPositions(), message.classNameList.ToList(), message.idList.ToList()));
        room.OnMessage<CharacterMessage>("Action", (message) => clientObserver.Action(message));
        room.OnMessage<NullReferenceException>("End_Turn", (message) => clientObserver.EndTurn());
    }

    public void RegisterObserver(ClientObserver o) => clientObserver = o;
    public void RegisterObserver(LobbyObserver o) => lobbyObserver = o;

    private void OnApplicationQuit() {
        if(room != null) room.Leave();
    }

    public ColyseusRoom<ConnectionState> GetCurrentRoom() => room;
    public bool CanStartGame() => canStartGame;
    public void SendAction(CharacterMessage modifiedCharacters) => room.Send("Action", modifiedCharacters);
    public void SendEndTurn() => room.Send("End_Turn", new {});
    public void SendInitialization() => room.Send("Initialization", new {});

    public string GetId() => room.SessionId;
    class Walls 
    {
        public bool[][] walls;

        public bool[,] GetWalls()
        {
            int xSize = this.walls.Length;
            int zSize = this.walls[0].Length;
            bool[,] walls = new bool[xSize,zSize];
            for (int x = 0; x < xSize; x++)
                for (int z = 0; z < zSize; z++)
                    walls[x,z] = this.walls[x][z];
            return walls;
        }
    }
    public class CharactersInitialization
    {
        public Position[] positions;
        public string[] classNameList;
        public string[] idList;

        public List<(int x, int z)> GetPositions() => positions.Select(position => (position.x, position.z)).ToList();
    }
    public class Position {
        public int x;
        public int z;
    }

    class Creator
    {
        public bool canStartGame;
    }

    class Client
    {
        public string id;
    }
}
