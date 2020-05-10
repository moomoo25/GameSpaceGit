using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public interface IGameManager
{
    void GameManagerSetting();
}
public class GameManager : IGameManager
{
   
    public void GameManagerSetting()
    {
        Debug.Log("AStar");
    }
}
