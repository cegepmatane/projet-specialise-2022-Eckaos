using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : ControlledCharacterObserver
{

    public Slider slider;
    public Text text;

    public override void UpdateUI(Character character)
    {
        slider.value = character.currentLifePoints;
        slider.maxValue = character.classData.lifePoints;
        text.text = slider.value +"/"+slider.maxValue;
    }

}
