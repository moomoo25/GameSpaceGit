using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DelayStart : MonoBehaviourPunCallbacks
{
    private PhotonView myPhotonView;
    public int multiplayerSceneIndex;
    public int menuSceneIndex;
    private int playerCount;
    private int roomSize;
    public int minPlayerToStart;
    public Text playerCountDisplay;
    public Text timerToStartDisplay;

    private bool readyToCountDown;
    private bool readyToStart;
    private bool startingGame;

    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGametimer;

    public float maxWaitTime;
    public float maxFullGameWaitTime;
    // Start is called before the first frame update
    void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        fullGametimer = maxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;
        PlayerCountUpdate();
    }
    void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerCountDisplay.text = playerCount + ":" + roomSize;
        if (playerCount == roomSize)
        {
            readyToStart = true;
        }
        else if(playerCount >= minPlayerToStart)
        {
            readyToCountDown = true;
        }
        else
        {
            readyToCountDown = false;
            readyToStart = false;
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
        if (PhotonNetwork.IsMasterClient)
        {
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
        }
    }
    [PunRPC]
    private void RPC_SendTimer(float TimeIn)
    {
        timerToStartGame = TimeIn;
        notFullGameTimer = TimeIn;
        if (TimeIn < fullGametimer)
        {
            fullGametimer = TimeIn;
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }
    // Update is called once per frame
    void Update()
    {
        WaitingForMorePlayer();
    }
    void WaitingForMorePlayer()
    {
        if (playerCount <= 1)
        {
            ResetTimer();
        }
        if (readyToStart)
        {
            fullGametimer -= Time.deltaTime;
            timerToStartGame = fullGametimer;
        }
        else if (readyToCountDown)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }
        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;
        if (timerToStartGame <= 0f)
        {
            if (startingGame)
            {
                return;
            }
            StartGame();
        }
    }
    void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGametimer = maxFullGameWaitTime;
    }
    void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }
    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}
