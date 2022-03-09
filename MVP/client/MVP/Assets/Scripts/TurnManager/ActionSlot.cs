using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class ActionSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SkillAction action;
    private Image image;

    public Text skillNameText;
    public Text skillDescriptionText;

    public void AddAction(SkillAction action)
    {
        this.action = action;
        skillDescriptionText.transform.parent.gameObject.SetActive(false);
        GetComponentInChildren<Text>().text = action.GetSkill().skillName.ToUpper()[0].ToString();
    }
    
    public void GetSelectableTiles() => this.action.GetSelectableTiles();
    
    public void OnPointerEnter(PointerEventData p)
    {
        skillDescriptionText.transform.parent.gameObject.SetActive(true);
        skillDescriptionText.text = action.GetSkill().description;
        skillNameText.text = action.GetSkill().skillName;
    }

    public void  OnPointerExit(PointerEventData p)
    {
        skillDescriptionText.transform.parent.gameObject.SetActive(false);
        skillDescriptionText.text= "";
        skillNameText.text = "";
    }
}
