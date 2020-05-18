using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    public GameObject quickStartButton;
    public GameObject quickCancleButton;
    public int roomSize;
    private void Awake()
    {
        quickStartButton.SetActive(false);
        quickCancleButton.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        quickStartButton.SetActive(true);
        quickCancleButton.SetActive(false);
    }
    public void QuickStart()
    {
        quickStartButton.SetActive(false);
        quickCancleButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("QuickStart");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
    }
    void CreateRoom()
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        Debug.Log(randomRoomNumber);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room try again");
        CreateRoom();
    }
    public void QuickCancel()
    {
        quickStartButton.SetActive(true);
        quickCancleButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }
}
