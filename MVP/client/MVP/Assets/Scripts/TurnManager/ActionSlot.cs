using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSlot : MonoBehaviour
{
    private Action action;

    public void AddAction(Action action) => this.action = action;
    public void GetSelectableTiles() => this.action.GetSelectableTiles();
    
}
