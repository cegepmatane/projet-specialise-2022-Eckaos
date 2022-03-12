using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ControlledCharacter : MonoBehaviour //SetUp UI => Need Character && MovementAction && SkillAction
{
    private Character currentCharacter;
    private MovementAction movement;
    private SkillAction skillAction;
    public TurnManager turnManager;
    private TileMap tileMap;
    private IGameClient client;
    public List<ControlledCharacterObserver> UIObservers;

    void Update()
    {
        if(currentCharacter == null) return;
        if(client.GetId() != currentCharacter.playerId) return;
        foreach (var UIObserver in UIObservers)
            UIObserver.UpdateUI(currentCharacter);
        if(!turnManager.IsNextTurnSameAsLastTurn()) turnManager.CreateTurnList();
        if(currentCharacter.currentActionPoint <= 0 && currentCharacter.currentMovementPoint <= 0) return;
        movement.GetSelectableTiles(!skillAction.IsSelecting());
        skillAction.Execute();
    }

    public void ChangeCurrentCharacter()
    {
        tileMap.ResetHighlight();
        if(currentCharacter != null)currentCharacter.Reset();
        currentCharacter = turnManager.GetNextTurn();
        movement = currentCharacter.GetComponent<MovementAction>();
        skillAction = currentCharacter.GetComponent<SkillAction>();
        movement.SetTileMap(tileMap);
        skillAction.SetTileMap(tileMap);
        foreach (var UIObserver in UIObservers)
            if(currentCharacter != null)
                UIObserver.UpdateUI(currentCharacter);
    }
    public Character GetCurrentCharacter() => currentCharacter;
    public void SetTileMap(TileMap tileMap) => this.tileMap = tileMap;
    public TileMap GetTileMap() => tileMap;

    public IGameClient SetGameClient(IGameClient gameClient) => client = gameClient;
    public void NextTurn()
    {
        if(client.GetId() != currentCharacter.playerId) return;
        client.SendEndTurn();
    }
}
