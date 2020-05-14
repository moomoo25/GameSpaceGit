using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class BarsSetting : MonoBehaviour
{
     private UIManager uiManager;
    private GameManager gameManager;
    [Header("Race")]
    public Image raceIcon;
    public Text raceName;
    [Header("Hp")]
    public Image hpBar;
    public Text hpText;
    [Header("Stamina")]
    public Image staminaBar;
    public Text staminaText;
    [Header("Mana")]
    public Image manaBar;
    public Text manaText;


    [Inject]
    public void SettingUIManager(UIManager uIManager_)
    {
        uiManager = uIManager_;
        uiManager.SetRaceIconAndText(raceIcon, raceName);
    }
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (uiManager != null)
        {
            staminaBar.transform.localScale = new Vector2(uiManager.GetPlayerStat().stamina / uiManager.GetPlayerStat().maxStamina, 1);
            staminaText.text = "" + (int)uiManager.GetPlayerStat().stamina + "/" + uiManager.GetPlayerStat().maxStamina;
        }
    
   
    }
 
}
