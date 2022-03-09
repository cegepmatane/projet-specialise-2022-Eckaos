using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledCharacter : MonoBehaviour
{
    private Character currentCharacter;
    public TurnManager turnManager;
    private Action actionToUse;

    public ActionPanel actionPanel;
    public HealthBar healthBar;
    public ActionPointIndicator actionPointIndicator;
    public MovementPointIndicator movementPointIndicator;


    // Update is called once per frame
    private void Start() {
        ChangeCurrentCharacter();
    }

    void Update()
    {
        movementPointIndicator.text.text = currentCharacter.currentMovementPoint+"";
        actionPointIndicator.text.text = currentCharacter.currentActionPoint+"";
        if(!turnManager.IsNextTurnSameAsLastTurn()) turnManager.CreateTurnList();
        if(currentCharacter.currentActionPoint <= 0 && currentCharacter.currentMovementPoint <= 0) return;
        actionToUse = GetActionToUse();
        if(actionToUse == null) return;
        actionToUse.Execute();
        
    }

    private void ChangeCurrentCharacter()
    {
        TileMap.GetInstance().ResetHighlight();
        if(currentCharacter != null)currentCharacter.Reset();
        currentCharacter = turnManager.GetNextTurn();
        actionPanel.ActivatePanel(GetActions());
        healthBar.slider.maxValue = currentCharacter.classData.lifePoints;
        healthBar.SetHealth(currentCharacter.currentLifePoints);
        movementPointIndicator.text.text = currentCharacter.currentMovementPoint+"";
        actionPointIndicator.text.text = currentCharacter.currentActionPoint+"";
    }

    public Action GetActionToUse()
    {
        if(currentCharacter == null) return null;
        foreach (Action action in currentCharacter.skillActions)
            if(action.IsSelecting() && !currentCharacter.movementAction.IsExecuting())
                return action; 
        if(currentCharacter.movementAction.IsValidForUse())
            return currentCharacter.movementAction;
        return null;
    }

    public Character GetCurrentCharacter() => currentCharacter;
    public bool isExecutingAction() => actionToUse.IsExecuting();
    public void NextTurn() => ChangeCurrentCharacter();
    public List<SkillAction> GetActions() => currentCharacter.skillActions;
}
