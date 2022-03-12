using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ActionSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SkillAction action;
    private Skill skill;
    private Image image;
    public Text skillNameText;
    public Text skillDescriptionText;

    public void AddAction(SkillAction action, Skill skill)
    {
        this.action = action;
        this.skill = skill;
        GetComponentInChildren<Text>().text = skill.skillName.ToUpper()[0].ToString();
    }
    
    public void Trigger()
    {
        if(action.IsSelecting()) Deactivate();
        else Activate();
    }

    private void Activate()
    {
        this.action.SetSkill(this.skill);
        this.action.GetSelectableTiles();
    }

    private void Deactivate() 
    {
        action.ResetHighlight();
        action.Selecting(false);
    }
    public void OnPointerEnter(PointerEventData p)
    {
        skillDescriptionText.transform.parent.gameObject.SetActive(true);
        skillDescriptionText.text = skill.description;
        skillNameText.text = skill.skillName;
    }

    public void  OnPointerExit(PointerEventData p)
    {
        skillDescriptionText.transform.parent.gameObject.SetActive(false);
        skillDescriptionText.text= "";
        skillNameText.text = "";
    }
}
