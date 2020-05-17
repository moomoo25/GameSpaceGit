using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NetworkController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        //  base.OnConnectedToMaster();
        Debug.Log("we are now connected to the" + PhotonNetwork.CloudRegion + " server!");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
