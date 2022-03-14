using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LobbyObserver
{
    public void Start(List<(string roomId, int clientNumber)> roomList);
    public void Add(string roomId, int clientNumber);
    public void Remove(string roomId);
}
