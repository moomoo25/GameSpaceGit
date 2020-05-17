using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public interface IGameManager
{
    void GameManagerSetting();
    void SetRaceUI(Image icon, Text text, Sprite s, string raceName);
}
public class GameManager : IGameManager
{
   public Transform[] spawnpoints;
  //  public ;
    public void GameManagerSetting()
    {
        Debug.Log("AStar");
    }
    public void SetRaceUI(Image icon, Text RaceText, Sprite s, string raceName)
    {
        Debug.Log(icon.name);
        icon.sprite = s;
        RaceText.text = raceName;
    }
}
