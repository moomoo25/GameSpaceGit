using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class BarsSetting : MonoBehaviour
{
     private UIManager uiManager;
    
    [Header("Stamina")]
    public Image staminaBar;
    public Text staminaText;


    [Inject]
    public void SettingUIManager(UIManager uiManager_)
    {
       this.uiManager = uiManager_;
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
        else
        {
            print("GG");
        }
   
    }
}
