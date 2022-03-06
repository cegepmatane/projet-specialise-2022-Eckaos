using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnManager : MonoBehaviour
{
    private List<Character> characters;
    private List<int> turnMeters;
    private int indexOfLastTurn;

    private void Start() {
        characters = new List<Character>();
        foreach (GameObject c in GameObject.FindGameObjectsWithTag("Character"))
            characters.Add(c.GetComponent<Character>());
        turnMeters = new List<int>();
        foreach (Character c in characters)
            turnMeters.Add(0);
    }
    
    public Character GetNextCharacterToPlay()
    {
        while (!turnMeters.Exists(meter => meter >= 100))
        {
            for (int i=0; i<turnMeters.Count; i++)
                turnMeters[i] += (int)characters[i].classData.speed.value;
        }
        int index = turnMeters.FindIndex(meter => meter >= 100);
        turnMeters[index] = 0;
        return characters[index];
    }
}
