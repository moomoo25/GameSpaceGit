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
            SetSpawnPoint();
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PlayerPrefs", "Player"), spawnPoint.position, spawnPoint.rotation, 0);
            TpsController tpsController = myAvatar.GetComponent<TpsController>();
            tpsController.SettingUIManager(uIManager);
            tpsController.isProcess = true;
            tpsController.SetUpPlayer("Human", TestController.singleton.cac, TestController.singleton.skill, TestController.singleton.colorIn);
      
            myAvatar.transform.position = spawnPoint.transform.position;
            myAvatar.transform.rotation = spawnPoint.transform.rotation;
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

    public void SetSpawnPoint()
    {
        spawnPoint = gameManager.spawnpoints[PhotonNetwork.PlayerList.Length - 1];
    }

    public void SetPlayerModelInfo()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}