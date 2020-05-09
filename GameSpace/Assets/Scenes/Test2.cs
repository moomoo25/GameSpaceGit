using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Test2 : MonoBehaviour
{
    GameManager gameManager;

    [Inject]
    public void SetUp(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private void Start()
    {
        gameManager.GameManagerSetting();
    }
}
