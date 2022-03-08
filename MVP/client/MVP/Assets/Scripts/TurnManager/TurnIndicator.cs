using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnIndicator : MonoBehaviour
{
    private List<Image> indicators;
    private float offset = -70;
    private int nbOffset = 0;

    private Vector2 indicatorSize = new Vector2(60, 90);
    public RectTransform turnIndicatorPosition;

    private void Start() {
        
    }

    public void UpdateIndicator(List<Character> characters, Character character)
    {
        if(indicators != null)
            foreach (Image img in indicators)
                DestroyImmediate(img.gameObject);
        indicators = new List<Image>();
        nbOffset = 0;
        for (int i = characters.Count - 1; i >= 0 ; i--)
        {
            Image img = CreateCharacterIndicator();
            if(characters[i]==character) img.color = Color.cyan;
            indicators.Add(img);
        }

        turnIndicatorPosition.sizeDelta = new Vector2(indicators.Count * indicatorSize.x + (indicators.Count+1) * 10 ,turnIndicatorPosition.sizeDelta.y);
        turnIndicatorPosition.ForceUpdateRectTransforms();
    }

    private Image CreateCharacterIndicator()
    {   
        Image image = CreateImage();
        RectTransform pos = image.GetComponent<RectTransform>();
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.layer = 5;
        cube.transform.localScale = new Vector3(30,30,30);
        cube.transform.localPosition = new Vector3(0,0,-200);
        cube.transform.rotation = Quaternion.Euler(-15,45,-15);
        cube.GetComponent<Renderer>().material.color = Color.red;
        cube.transform.SetParent(pos);
        pos.SetParent(turnIndicatorPosition);
        SetUpImage(pos);
        pos.ForceUpdateRectTransforms();
        nbOffset++;
        return image;
    }

    private Image CreateImage()
    {
        GameObject obj = new GameObject("Indicator");
        Image img = obj.AddComponent<Image>();
        return img;
    }
    
    private void SetUpImage(RectTransform pos)
    {
        pos.sizeDelta = indicatorSize;
        pos.localScale = new Vector3(1,1,1);
        pos.localPosition = new Vector3(-40+nbOffset*offset,0,0);
        pos.anchorMin = new Vector2(1, 0.5f);
        pos.anchorMax = new Vector2(1, 0.5f);
        pos.pivot = new Vector2(0.5f, 0.5f);
    }
    
}
