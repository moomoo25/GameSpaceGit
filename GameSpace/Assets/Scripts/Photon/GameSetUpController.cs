using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Zenject;
using Photon.Pun;
public class GameSetUpController : MonoBehaviour
{
    public static GameSetUpController singleton;
    private GameManager gameManager;
    private UIManager uiManager;
    private DefaultInstaller.PlayerStat[] refStats;
    private MyGameSettingInstaller.Skills[] refSkill;
    private Color[] characterColor;
    private MyGameSettingInstaller.AllClass[] classes;
    [Inject]
    public void SettingGameManager(GameManager gameManager_)
    {
        gameManager = gameManager_;
    }
    [Inject]
    public void SettingGameManager(UIManager uimanager_)
    {
        uiManager = uimanager_;
    }
    [Inject]
    public void SettingPlayerStat(DefaultInstaller.PlayerStat[] playerStats_)
    {
        refStats = playerStats_; // can use 
    }
    [Inject]
    public void SetUpSkillAndClass(MyGameSettingInstaller.Skills[] refSkills_, MyGameSettingInstaller.AllClass[] allPlayerClasses)
    {
        refSkill = refSkills_; // can use 
        classes = allPlayerClasses;
    }
    [Inject]
    public void SetupColor(Color[] characterColor_)
    {
        characterColor = characterColor_;
    }
    private void Awake()
    {
        GameSetUpController.singleton = this;
    }
    void Start()
    {
            CreatePlayer();
    }
    void CreatePlayer()
    {
       Debug.Log("create player");
       GameObject player = PhotonNetwork.Instantiate(Path.Combine("PlayerPrefs", "PlayerPhotonView"), Vector3.zero, Quaternion.identity,0) as GameObject;
        if (player != null)
        {
            PhotonPlayer photonPlayer = player.GetComponent<PhotonPlayer>();
            if (photonPlayer != null)
            {
                photonPlayer.SetSpawnPoint(gameManager.spawnpoints[0]);
                photonPlayer.SetUiManager(this.uiManager);
                photonPlayer.SetPlayerModelInfo(refStats, refSkill, characterColor, classes);
            }
           
        }
     
    }
    // Update is called once per frame
    public void forceSetUp(TpsController player)
    {
        player.SetPlayerModelInfo(refStats, refSkill, characterColor, classes);
    }

}
