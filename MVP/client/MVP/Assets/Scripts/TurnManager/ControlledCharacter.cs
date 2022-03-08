using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledCharacter : MonoBehaviour
{
    private Character currentCharacter;
    public TurnManager turnManager;
    private Action actionToUse;

    public ActionPanel actionPanel;


    // Update is called once per frame
    private void Start() {
        ChangeCurrentCharacter();
    }

    void Update()
    {
        if(currentCharacter.currentActionPoint <= 0 && currentCharacter.currentMovementPoint <= 0) return;
        actionToUse = GetActionToUse();
        if(actionToUse == null) return;
        actionToUse.Execute();
    }

    private void ChangeCurrentCharacter()
    {
        TileMap.GetInstance().ResetHighlight();
        if(currentCharacter != null)currentCharacter.Reset();
        currentCharacter = turnManager.GetTurn();
        actionPanel.ActivatePanel(GetActions());
    }

    public Action GetActionToUse()
    {
        foreach (Action action in currentCharacter.skillActions)
            if(action.IsSelecting() && !currentCharacter.movementAction.IsExecuting())
                return action;  
        if(currentCharacter.movementAction.IsValidForUse())
            return currentCharacter.movementAction;
        return null;
    }

    public bool isExecutingAction() => actionToUse.IsExecuting();
    public void NextTurn() => ChangeCurrentCharacter();
    public List<Action> GetActions() => currentCharacter.skillActions;
}
