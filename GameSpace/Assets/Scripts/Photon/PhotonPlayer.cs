using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Zenject;
public class PhotonPlayer : MonoBehaviour
{
    PhotonView pv;
    public GameObject myAvatar;
    private UIManager uIManager;
    private Transform spawnPoint;

    private DefaultInstaller.PlayerStat[] refStats;
    private MyGameSettingInstaller.Skills[] refSkill;
    private Color[] characterColor;
    private MyGameSettingInstaller.AllClass[] classes;
    // Start is called before the first frame update
    void Start()
    {

        pv = GetComponent<PhotonView>();
       // int spawnPicker = Random.Range(0,gameManager.spawnpoints.Length);
        if (pv.IsMine)
        {
            myAvatar= PhotonNetwork.Instantiate(Path.Combine("PlayerPrefs", "Player"), Vector3.zero, Quaternion.identity,0);
            myAvatar.transform.position = spawnPoint.position;
            myAvatar.transform.rotation = spawnPoint.rotation;
            TpsController tpsController = myAvatar.GetComponent<TpsController>();
            tpsController.SettingUIManager(this.uIManager);
            tpsController.SetPlayerModelInfo(refStats,refSkill,characterColor,classes);
            tpsController.isProcess = true;
        }
      
      

    }
    public void SetUiManager(UIManager uIManager_)
    {
        uIManager = uIManager_;
    }
    public void SetSpawnPoint(Transform spawnPoint_)
    {
        spawnPoint = spawnPoint_;

    }
    public void SetPlayerModelInfo(DefaultInstaller.PlayerStat[] refStats_, MyGameSettingInstaller.Skills[] refSkill_,Color[] characterColor_, MyGameSettingInstaller.AllClass[] classes_)
    {
        refStats = refStats_;
        refSkill = refSkill_;
        characterColor = characterColor_;
        classes = classes_;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
