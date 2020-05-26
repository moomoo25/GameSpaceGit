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
    public void SettingGameManagerAndUiManager(GameManager gameManager_,UIManager uIManager_)
    {
        gameManager = gameManager_; // can use 
        uiManager = uIManager_;
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
               // TestController.singleton.gameState = GameState.Team;
                photonPlayer.SetUpCharacter(gameManager, uiManager, playerPref, PlayFabController.singleton.refStats, PlayFabController.singleton.refSkill, PlayFabController.singleton.characterColor, PlayFabController.singleton.classes);
            }
           
        }
     
    }
 


}
