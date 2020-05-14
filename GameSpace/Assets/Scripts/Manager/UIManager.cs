using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public interface IUIManager
{
    DefaultInstaller.PlayerStat  GetPlayerStat();
    void SetPlayerStat(DefaultInstaller.PlayerStat playerStat_);
    void InitSetRace(Sprite raceIconRef, string iconNameRef);
    void SetRaceIconAndText(Image showIconImage, Text showText);
}
public class UIManager : IUIManager
{
    private GameManager gameManager;
    public Image raceIcon;
    public Text raceName;
    public DefaultInstaller.PlayerStat playerStat;
    public DefaultInstaller.PlayerStat GetPlayerStat()
    {
        return playerStat;
    }
    [Inject]
    public void SettingGameManager(GameManager gameManager_)
    {
        gameManager = gameManager_;
    }
    public void SetPlayerStat(DefaultInstaller.PlayerStat playerStat_)
    {
        playerStat = playerStat_;
    }
    public void SetRaceIconAndText(Image showIconImage, Text showText)
    {
        raceIcon = showIconImage;
        Debug.Log(raceIcon);
        raceName = showText;
    }
    public void InitSetRace(Sprite raceIconRef,string iconNameRef)
    {
        if (raceIcon == null)
        {
            return;
        }
            gameManager.SetRaceUI(raceIcon, raceName, raceIconRef, iconNameRef);
       

        
    }

}
