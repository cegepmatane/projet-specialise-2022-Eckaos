using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameClient
{
    // Start is called before the first frame update
    public void SendAction(CharacterMessage messages);
    public void SendEndTurn();

    public string GetId();
}
