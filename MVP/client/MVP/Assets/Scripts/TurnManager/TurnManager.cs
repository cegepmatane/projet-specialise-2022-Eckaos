using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<Character> characters;
    private Character character;
    private int turn = 0;
    public TurnIndicatorList turnIndicators;
    private void Start() {
        characters = new List<Character>();
    }

    public Character GetNextTurn()
    {
        if(characters.Count <= 0)CreateTurnList();
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
        characters.OrderByDescending(c=> c.classData.speed);
        turnIndicators.UpdateIndicator(characters, characters[turn]);
        if(characters.Count <= 0) return;//TODO QUITTER;
    }

    public List<Character> GetCharacters() => characters;

}
