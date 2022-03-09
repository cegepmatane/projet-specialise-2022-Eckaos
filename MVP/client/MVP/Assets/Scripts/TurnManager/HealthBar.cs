using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Text text;

    public void SetHealth(int lifePoint)
    {
        slider.value = lifePoint;
        text.text = lifePoint +"/"+slider.maxValue;
    }

}
