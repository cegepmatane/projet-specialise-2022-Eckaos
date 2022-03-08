using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TurnIndicatorList : MonoBehaviour
{
    private static float offset = -70;
    public RectTransform turnIndicatorListPosition;
    private List<TurnIndicator> indicators;

    public void UpdateIndicator(List<Character> characters, Character character)
    {
        if(indicators == null || characters.Count != indicators.Count)
            UpdateIndicator(characters);
        indicators.ForEach(indicator => indicator.panel.color = Color.white);
        indicators[characters.IndexOf(character)].panel.color = Color.cyan;
    }

    public void UpdateIndicator(List<Character> characters)
    {
        ResetIndicators();
        indicators = new List<TurnIndicator>();
        turnIndicatorListPosition.sizeDelta = new Vector2(characters.Count * TurnIndicator.size.x + (characters.Count+1) * 10 ,turnIndicatorListPosition.sizeDelta.y);
        for (int i = 0; i < characters.Count ; i++)
            indicators.Add(new TurnIndicator(turnIndicatorListPosition,characters[i], -40+(characters.Count-1-i)*offset));
        
        turnIndicatorListPosition.ForceUpdateRectTransforms();
    }

    private void ResetIndicators()
    {
        if(indicators != null)
            foreach (TurnIndicator turnIndicator in indicators)
                DestroyImmediate(turnIndicator.panel);
    }
    
}
