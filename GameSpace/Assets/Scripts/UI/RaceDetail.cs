using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RaceDetail : MonoBehaviour
{
    public TpsController player;
    public Text leaderBoardText;
    private Text raceDetailText;
    // Start is called before the first frame update
    void Start()
    {
        raceDetailText = GetComponent<Text>();
      
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayFabController.singleton != null)
        {
            leaderBoardText.text = PlayFabController.singleton.leaderBoardText;
        }
        raceDetailText.text = "MaxHP:" + player.maxHealth + " MaxStamina:" + player.maxStamina + " MaxMana:" + player.maxMana + " PlayerAttackDamage:" + player.playerDamage;
    }
}
