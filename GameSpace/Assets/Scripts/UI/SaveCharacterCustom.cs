using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveCharacterCustom : MonoBehaviour
{
    public TpsController playerSettingModel;
    private PlayFabController playFabController;

    public void OnSaveCharacterSetting()
    {
        if (PlayFabController.singleton != null)
        {
            PlayFabController.singleton.EditPlayerData(playerSettingModel.playerClass, playerSettingModel.playerRace, playerSettingModel.playerSkillName, playerSettingModel.colorIndex.ToString());
        }
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(2);
    }
}
