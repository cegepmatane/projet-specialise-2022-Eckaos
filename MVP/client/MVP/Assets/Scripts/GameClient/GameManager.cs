using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ClientObserver
{
    private GameClient client;
    [SerializeField]
    private MapGenerator mapGenerator;
    [SerializeField]
    private CharacterGenerator characterGenerator;
    [SerializeField]
    private TurnManager turnManager;
    [SerializeField]
    private ControlledCharacter controlledCharacter;
    private TileMap tileMap;
    private List<Character> characters;
    private void Start() {
        client = GameClient.GetInstance(); 
        client.RegisterObserver(this);
        client.SendInitialization();
        characters = new List<Character>();
        controlledCharacter.SetGameClient(client);
    }
    public void InitializeMap(bool[,] walls)
    {
        tileMap = mapGenerator.GenerateMap(walls);
        controlledCharacter.SetTileMap(tileMap);
    }
    public void InitializeCharacters(List<(int x, int z)> positions, List<string> classNameList, List<string> idList)
    {
        characters = characterGenerator.GenerateCharacters(positions, classNameList, idList);
        foreach (var c in characters)
        {
            tileMap.SetCharacter((int)c.transform.position.x, (int)c.transform.position.z, c.gameObject);
            MovementAction movement = c.GetComponent<MovementAction>();
            movement.SetTileMap(tileMap);
            movement.SetGameClient(client);
            SkillAction skillAction = c.GetComponent<SkillAction>();
            skillAction.SetTileMap(tileMap);
            skillAction.SetGameClient(client);
        }
        controlledCharacter.ChangeCurrentCharacter();
    }
    public void Action(CharacterMessage message)
    {
        GameObject obj = tileMap.GetTile(message.startX, message.startZ).player;
        if(obj == null) 
            return;
        if(message.destX != message.startX || message.startZ != message.destZ)
        {
            tileMap.GetTile(message.startX, message.startZ).player = null;
            obj.GetComponent<MovementAction>().SetUp(message.destX, message.destZ);
            tileMap.GetTile(message.destX, message.destZ).player = obj;
        }
        obj.GetComponent<Character>().currentLifePoints= message.lifePoints;
    }

    public void EndTurn() => controlledCharacter.ChangeCurrentCharacter();
}
