using Photon.Pun;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView pv;
    private GameObject playerPref;
    public GameObject myAvatar;
    private UIManager uIManager;
    private GameManager gameManager;
    private Transform spawnPoint;

    private DefaultInstaller.PlayerStat[] refStats;
    private MyGameSettingInstaller.Skills[] refSkill;
    private Color[] characterColor;
    private MyGameSettingInstaller.AllClass[] classes;

    // Start is called before the first frame update
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            int IndexById = pv.ViewID / 1000;
            spawnPoint = gameManager.spawnpoints[IndexById-1];
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PlayerPrefs", "Player"), spawnPoint.position, spawnPoint.rotation, 0);
         
            TpsController tpsController = myAvatar.GetComponent<TpsController>();
            tpsController.SettingUIManager(uIManager);
            tpsController.isProcess = true;

            IndexById = IndexById % 2;
            if (IndexById == 0)
            {
                IndexById = 2;
            }
            tpsController.SetUpPlayer(PlayFabController.singleton.playerRace, PlayFabController.singleton.playerClass, PlayFabController.singleton.playerSkill, PlayFabController.singleton.playerColorIndex, IndexById);
       
          //  myAvatar.transform.position = spawnPoint.transform.position;
          //  myAvatar.transform.rotation = spawnPoint.transform.rotation;
        }
    }
   
    public void SetUpCharacter(GameManager gameManager_, UIManager uIManager_, GameObject player_, DefaultInstaller.PlayerStat[] refStats_, MyGameSettingInstaller.Skills[] refSkill_, Color[] characterColor_, MyGameSettingInstaller.AllClass[] classes_)
    {
        uIManager = uIManager_;
        gameManager = gameManager_;
        playerPref = player_;
        refStats = refStats_;
        refSkill = refSkill_;
        characterColor = characterColor_;
        classes = classes_;
    }

}