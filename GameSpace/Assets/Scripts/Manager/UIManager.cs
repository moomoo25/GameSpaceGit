using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public interface IUIManager
{
   
    void SetPlayerSkill(MyGameSettingInstaller.Skills skill_);
    MyGameSettingInstaller.Skills GetPlayerSkill();
   // void SetPlayerStat(DefaultInstaller.PlayerStat playerStat_);
    void InitSetRace(Sprite raceIconRef, string iconNameRef);
    void SetRaceIconAndText(Image showIconImage, Text showText);
}
public class UIManager : IUIManager
{
    private GameManager gameManager;
    public Image raceIcon;
    public Text raceName;
    public MyGameSettingInstaller.Skills skills;
    public float cooldownTime;
    public DefaultInstaller.PlayerStat playerStat;
    [Inject]
    public void SettingGameManager(GameManager gameManager_)
    {
        gameManager = gameManager_;
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
    public void SetPlayerSkill(MyGameSettingInstaller.Skills skill_)
    {
        this.skills = skill_;
    }
    public MyGameSettingInstaller.Skills GetPlayerSkill()
    {
        return this.skills;
    }

}
