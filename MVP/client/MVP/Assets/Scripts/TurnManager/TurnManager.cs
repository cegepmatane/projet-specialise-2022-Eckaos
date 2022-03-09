using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<Character> characters;
    private Character character;

    private int turn = 0;

    public TurnIndicatorList turnIndicators;

    public Character GetNextTurn()
    {
        if(characters == null)CreateTurnList();
        character = characters[turn];
        turnIndicators.UpdateIndicator(characters, character);
        if(turn < characters.Count-1)turn++;
        else turn = 0;
        return character;
    }

    public bool IsNextTurnSameAsLastTurn() => characters.Count == GameObject.FindGameObjectsWithTag("Character").Length;

    public void CreateTurnList()
    {
        characters = GameObject.FindGameObjectsWithTag("Character").Select(charObj => charObj.GetComponent<Character>()).ToList();
        if(character != null) turn = characters.IndexOf(character);
        characters.Sort(SortBySpeed);
        turnIndicators.UpdateIndicator(characters, characters[turn]);
        if(characters.Count <= 0) return;//TODO QUITTER;
    }


    public int SortBySpeed(Character a, Character b)
    {
        if(a.classData.speed > b.classData.speed) return -1;
        else if(a.classData.speed < b.classData.speed) return 1;
        else return 0;
    }

}
