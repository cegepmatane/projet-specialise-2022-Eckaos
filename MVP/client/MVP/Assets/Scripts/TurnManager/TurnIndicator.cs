using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnIndicator
{
    public Image panel;
    private GameObject sprite;
    private RectTransform position;

    public static readonly Vector2 size = new Vector2(60,90);

    public TurnIndicator(RectTransform parent,Character character, float x)
    {
        panel = CreatePanel(parent,x);
        position.ForceUpdateRectTransforms();
        sprite = CreateSprite(character);
    }
    private Image CreatePanel(RectTransform parent,float x)
    {
        GameObject obj = new GameObject("Indicator");
        Image img = obj.AddComponent<Image>();
        position = img.GetComponent<RectTransform>();
        SetUp(parent,x);
        return img;
    }
    public void SetUp(RectTransform parent,float x)
    {
        position.SetParent(parent);
        position.sizeDelta = size;
        position.localScale = new Vector3(1,1,1);
        position.localPosition = new Vector3(x,0,0);
        position.anchorMin = new Vector2(1, 0.5f);
        position.anchorMax = new Vector2(1, 0.5f);
        position.pivot = new Vector2(0.5f, 0.5f);
    }

    private GameObject CreateSprite(Character character)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        SetUp(cube, character);
        return cube;
    }

    private void SetUp(GameObject cube, Character character)
    {
        cube.transform.SetParent(position);
        cube.layer = 5;
        cube.transform.localScale = new Vector3(30,30,30);
        cube.transform.localPosition = new Vector3(0,0,-200);
        cube.transform.rotation = Quaternion.Euler(-15,45,-15);
        cube.GetComponent<Renderer>().material = character.GetComponent<Renderer>().material;
    }
}
