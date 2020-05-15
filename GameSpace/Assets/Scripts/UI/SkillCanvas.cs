using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class SkillCanvas : MonoBehaviour
{
    private UIManager uIManager;
    public Image cooldownImg;
    public Image SkillIcon;
    public Text skillName;
    MyGameSettingInstaller.Skills skill;
  [Inject]
    public void SettingUIManager(UIManager uIManager_)
    {
        uIManager = uIManager_;
    }

    private void Update()
    {
        if (skill == null)
        {
            skill = uIManager.GetPlayerSkill();
        }
        else
        {
            SkillIcon.sprite = skill.skillIcon;
            skillName.text = skill.skillName;
        }
        
          
        
    
        cooldownImg.fillAmount = uIManager.cooldownTime;
    }
}
