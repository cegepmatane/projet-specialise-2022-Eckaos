using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ClassPicker : MonoBehaviour
{
    public List<Dropdown> dropdowns;

    private List<Class> classes;

    private ClassPickerObserver observer;

    void Start()
    {
        classes = Resources.LoadAll<Class>("Class").ToList();
        foreach (var dropdown in dropdowns)
        {
            foreach(var c in classes)
                dropdown.options.Add(new Dropdown.OptionData(c.className));
            dropdown.SetValueWithoutNotify(0);
        }
    }

    public string GetClass1(){
        return dropdowns[0].captionText.text;
    }

    public string GetClass2()
    {
        return dropdowns[1].captionText.text;
    }

    public void SetClass1(string className)
    {
        dropdowns[0].SetValueWithoutNotify(dropdowns[0].options.FindIndex(0, o => o.text.Equals(className)));
    }

    public void SetClass2(string className)
    {
        dropdowns[1].SetValueWithoutNotify(dropdowns[1].options.FindIndex(0, o => o.text.Equals(className)));
    }
    public void RegisterObserver(ClassPickerObserver observer) => this.observer = observer;
}
