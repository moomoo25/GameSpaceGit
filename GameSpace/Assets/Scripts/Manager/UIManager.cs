using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIManager
{
    PlayerStat GetPlayerStat();
    void SetPlayerStat(PlayerStat playerStat_);
}
public class UIManager : IUIManager
{
    public PlayerStat playerStat;
    public PlayerStat GetPlayerStat()
    {
        return playerStat;
    }
    public void SetPlayerStat(PlayerStat playerStat_)
    {
        playerStat = playerStat_;
    }
}
