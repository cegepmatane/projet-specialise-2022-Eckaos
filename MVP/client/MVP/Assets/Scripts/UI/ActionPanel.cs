using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanel : ControlledCharacterObserver
{
    private ActionSlot[] slots;
    public override void UpdateUI(Character character)
    {
        SkillAction action = character.GetComponent<SkillAction>();
        slots = GetComponentsInChildren<ActionSlot>();
        for (int i = 0; i < slots.Length; i++)
            slots[i].AddAction(action, character.GetSkill(i));
    }
}
