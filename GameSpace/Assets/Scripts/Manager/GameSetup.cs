using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class GameSetup : MonoBehaviour
{
    public Transform[] spawnPoint;
    private GameManager gameManager;
    // Start is called before the first frame update
    [Inject]
    public void SettingGameManager(GameManager gameManager_)
    {
        gameManager = gameManager_;
    }
    void Awake()
    {
        gameManager.spawnpoints = this.spawnPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
