
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    public GameObject StartLastManStandinButton;
    public GameObject StartTeamButton;
    public GameObject quickCancleButton;
    string mode = "";
    public int roomSize;
    private void Awake()
    {
        StartLastManStandinButton.SetActive(false);
        StartTeamButton.SetActive(false);
        quickCancleButton.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        StartLastManStandinButton.SetActive(true);
        StartTeamButton.SetActive(true);
        quickCancleButton.SetActive(false);
    }
    public void QuickStart(int modeIndex)
    {
        if (modeIndex==0)
        {
            TestController.singleton.gameState = GameState.LastManStanding;
            mode = "LastManStanding";
        }
        else
        {
            TestController.singleton.gameState = GameState.Team;
            mode = "Team";
        }
        StartTeamButton.SetActive(false);
        StartLastManStandinButton.SetActive(false);
        quickCancleButton.SetActive(true);

        //Hashtable expectedCustomRoomProperties = new Hashtable()
        //{
        //{ "Color",color },
        //{"Level", level }
        //};
        //PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, (byte)roomSize);

        PhotonNetwork.JoinRandomRoom();

        Debug.Log("QuickStart");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room");
        //string roomname = "ABC";
        //string color = "black";
        //string level = "1";
        //CreateRoom(roomname, MaxPlayerFromTable, color, level);
        CreateRoom();
    }
    void CreateRoom()
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        //Hashtable roomProps = new Hashtable() { { "Mode", mode } };
        //string[] roomPropsInLobby = {"Mode"};
        //roomOps.CustomRoomProperties = roomProps;
        //roomOps.CustomRoomPropertiesForLobby = roomPropsInLobby;
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
        StartLastManStandinButton.SetActive(true);
        quickCancleButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }
}
