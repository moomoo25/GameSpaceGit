using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestController : MonoBehaviour
{
    public static TestController singleton;
    public InputField className;
    public InputField skillName;
    public InputField colorName;
    public string cac;
    public string skill;
    public int colorIn;
    public DefaultInstaller.PlayerStat[] refStats;
    public MyGameSettingInstaller.Skills[] refSkill;
    public Color[] characterColor;
    public MyGameSettingInstaller.AllClass[] classes;
    private void OnEnable()
    {
        if (TestController.singleton == null)
        {
            TestController.singleton = this;
        }
        else
        {
            if (TestController.singleton != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        className.text = "Warrior";
        skillName.text = "Arrow Rain";
        colorName.text = "0";
        
    }

    public void SetUp()
    {
        cac = className.text;
        skill = skillName.text;
        colorIn = int.Parse(colorName.text);
    }
}
