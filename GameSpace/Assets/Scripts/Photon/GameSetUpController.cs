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
    private GameObject playerPref;


    [Inject]
    public void SetUpSkillAndClass(GameManager gameManager_, UIManager uimanager_,MyGameSettingInstaller.Skills[] refSkills_, MyGameSettingInstaller.AllClass[] allPlayerClasses, Color[] characterColor_, DefaultInstaller.PlayerStat[] playerStats_)
    {
        gameManager = gameManager_;
        uiManager = uimanager_;
        refSkill = refSkills_; // can use 
        classes = allPlayerClasses;
        characterColor = characterColor_;
        refStats = playerStats_; // can use 



    }
    [Inject]
    public void SettingPlayer(GameObject player_)
    {
        playerPref = player_; // can use 
    }
    private void Awake()
    {
        GameSetUpController.singleton = this;
        CreatePlayer();
    }
    void Start()
    {
        
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
                photonPlayer.SetUpCharacter(this.gameManager, this.uiManager, playerPref, refStats, refSkill, characterColor, classes);
                if (TestController.singleton != null)
                {
                    TestController.singleton.refSkill = refSkill;
                    TestController.singleton.refStats = refStats;
                    TestController.singleton.characterColor = characterColor;
                    TestController.singleton.classes = classes;
                }

            }
           
        }
     
    }
 


}
