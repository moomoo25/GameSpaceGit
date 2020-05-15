using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class BarsSetting : MonoBehaviour
{
    public static BarsSetting singleton;
    public TpsController player;
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
    void Awake()
    {
        singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            SetBar(staminaBar, staminaText, player.stamina, player.maxStamina);
            SetBar(hpBar, hpText, player.health, player.maxHealth);
            SetBar(manaBar, manaText, player.mana, player.maxMana);
        }
    
   
    }
 
    void SetBar(Image bar,Text valueText , float value , float maxValue)
    {
        bar.transform.localScale = new Vector2(value / maxValue, 1);
        valueText.text = "" + (int)value + "/" + maxValue;
    }
}
