using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIManager
{
    DefaultInstaller.PlayerStat  GetPlayerStat();
    void SetPlayerStat(DefaultInstaller.PlayerStat playerStat_);
}
public class UIManager : IUIManager
{
    public DefaultInstaller.PlayerStat playerStat;
    public DefaultInstaller.PlayerStat GetPlayerStat()
    {
        return playerStat;
    }
    public void SetPlayerStat(DefaultInstaller.PlayerStat playerStat_)
    {
        playerStat = playerStat_;
    }
}
