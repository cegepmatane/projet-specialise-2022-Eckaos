using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ClientObserver
{
    public void InitializeMap(bool[,] walls);
    public void InitializeCharacters(List<(int x, int z)> positions, List<string> className, List<string> idList);
    public void Action(CharacterMessage message);
    public void EndTurn();
}
