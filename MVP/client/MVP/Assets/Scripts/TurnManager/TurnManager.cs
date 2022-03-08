using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<Character> characters;

    private int turn = 0;

    public TurnIndicatorList turnIndicators;

    public Character GetNextTurn()
    {
        if(characters == null || characters.Any(character => character.currentLifePoints <= 0))CreateTurnList();
        Character character = characters[turn];
        turnIndicators.UpdateIndicator(characters, character);
        if(turn < characters.Count-1)turn++;
        else turn = 0;
        return character;
    }

    public bool IsNextTurnSameAsLastTurn() => characters.Count == GameObject.FindGameObjectsWithTag("Character").Length;

    public void CreateTurnList()
    {
        characters = new List<Character>();
        characters = GameObject.FindGameObjectsWithTag("Character").Select(charObj => charObj.GetComponent<Character>()).ToList();
        characters.Sort(SortBySpeed);
        if(characters.Count <= 0) return;//TODO QUITTER;
    }


    public int SortBySpeed(Character a, Character b)
    {
        if(a.classData.speed > b.classData.speed) return -1;
        else if(a.classData.speed < b.classData.speed) return 1;
        else return 0;
    }

}
