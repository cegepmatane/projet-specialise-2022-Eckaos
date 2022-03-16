using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WaitingRoomObserver
{
    void ChangeClass1UI(string className);
    void ChangeClass2UI(string className);
    void UpdatePlayers(string[] pseudos);
    void UpdateSpectators(string[] pseudos);
}
