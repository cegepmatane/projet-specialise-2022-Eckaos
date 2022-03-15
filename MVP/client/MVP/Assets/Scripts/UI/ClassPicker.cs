using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ClassPicker : MonoBehaviour
{
    public List<Dropdown> dropdowns;

    private List<Class> classes;

    void Start()
    {
        classes = Resources.LoadAll<Class>("Class").ToList();
        foreach (var dropdown in dropdowns)
            foreach(var c in classes)
                dropdown.options.Add(new Dropdown.OptionData(c.className));
    }
}
