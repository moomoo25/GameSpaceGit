using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
public class SaveCharacterCustom : MonoBehaviour
{
    public TpsController playerSettingModel;
    private UIManager uIManager;
    private PlayFabController playFabController;

    [Inject]
    public void SettingUIManager(UIManager uIManager_)
    {
        uIManager = uIManager_;
    }
    private void OnEnable()
    {
        playerSettingModel.SettingUIManager(uIManager);
    
    }
    private void Start()
    {
        playerSettingModel.SetUpPlayerOffline(PlayFabController.singleton.playerRace, PlayFabController.singleton.playerClass, PlayFabController.singleton.playerSkill, PlayFabController.singleton.playerColorIndex, 0);
    }
    public void OnSaveCharacterSetting()
    {
        if (PlayFabController.singleton != null)
        {
            PlayFabController.singleton.EditPlayerData(playerSettingModel.playerClass, playerSettingModel.playerRace, playerSettingModel.playerSkillName, playerSettingModel.colorIndex.ToString());
        }
    }
    public void SetRace(string race)
    {
        PlayFabController.singleton.playerRace = race;
        playerSettingModel.SetRace(race);
    }
    public void SetClass(string playerClass)
    {
        PlayFabController.singleton.playerClass = playerClass;
        playerSettingModel.SetClass(playerClass);
    }
     public void SetColor(int i)
    {
        PlayFabController.singleton.playerColorIndex = i;
        playerSettingModel.SetColor(i);
    }
    public void SetSkill(string skill)
    {
        PlayFabController.singleton.playerSkill = skill;
        playerSettingModel.SetSkill(skill);
    }
  
}
