using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnIndicator : MonoBehaviour
{
    private List<Image> indicators;
    private float offset = 45;
    private int nbOffset = 0;


    public void UpdateIndicator(List<Character> characters, Character character)
    {
        indicators = new List<Image>();
        nbOffset = 0;
        foreach (Character c in characters)
            indicators.Add(CreateImage());
    }

    private Image CreateImage()
    {
        GameObject obj = new GameObject("test");
        Image img = obj.AddComponent<Image>();
        RectTransform pos = obj.GetComponent<RectTransform>();
        pos.SetParent(transform);
        pos.localScale = new Vector3(0.4f, 0.5f, 1);
        pos.localPosition = new Vector3(120+offset*nbOffset,-120,0);
        pos.ForceUpdateRectTransforms();
        obj.SetActive(true);
        nbOffset++;
        return img;
    }
    
}
