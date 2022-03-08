using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    private ActionSlot[] slots;
    public void ActivatePanel(List<Action> actions)
    {
        slots = GetComponentsInChildren<ActionSlot>();
        for (int i = 0; i < slots.Length; i++)
            slots[i].AddAction(actions[i]);
    }
}
