using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementPointIndicator : ControlledCharacterObserver
{
    public Text text;

    public override void UpdateUI(Character character) => text.text = character.currentMovementPoint+"";
}
