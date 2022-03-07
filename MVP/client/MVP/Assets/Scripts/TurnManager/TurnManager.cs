using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<Character> characters;

    private int turn = 0;

    public TurnIndicator turnIndicator;

    private void Start() {
        characters = new List<Character>();
    }

    public Character GetTurn()
    {
        var characterList = GameObject.FindGameObjectsWithTag("Character");
        if(characters.Count != characterList.Length)CreateTurnList(characterList);
        Character character = characters[turn];
        turnIndicator.UpdateIndicator(characters, character);
        if(turn < characters.Count-1)turn++;
        else turn = 0;
        Debug.Log(character.classData.speed);
        return character;
    }

    public void CreateTurnList(GameObject[] characterList)
    {
        characters.Clear();
        characters = characterList.Select(charObj => charObj.GetComponent<Character>()).ToList();
        characters.Sort(SortBySpeed);
    }


    public int SortBySpeed(Character a, Character b)
    {
        if(a.classData.speed > b.classData.speed) return -1;
        else if(a.classData.speed < b.classData.speed) return 1;
        else return 0;
    }

}
