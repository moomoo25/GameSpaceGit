using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestController : MonoBehaviour
{
    public static TestController singleton;
    public List<TpsController> tpsControllers = new List<TpsController>();
    public InputField className;
    public InputField skillName;
    public InputField colorName;
    public string cac;
    public string skill;
    public int colorIn;
    public int teamWin;
    public GameState gameState;
    public int countPlayer;
    public bool isGameEnd;

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
    public bool CheckPlayerAlive(int playerCount)
    {
        int alivePlayer = playerCount;
        if (alivePlayer < 2)
        {
            return false;
        }

        if (gameState == GameState.LastManStanding)
        {
            if (tpsControllers.Count == 1)
            {
                return true;
            }
        }
        else
        {
            int teamACount = 0;
            int teamBCount = 0;
            for (int i = 0; i < tpsControllers.Count; i++)
            {
                if (tpsControllers[i].playerTeam==1)
                {
                    teamACount++;
                }
                else
                {
                    teamBCount++;
                }
            }
            if(teamACount==tpsControllers.Count)
            {
                teamWin = 1;
                return true;
            }
            if (teamBCount == tpsControllers.Count)
            {
                teamWin = 2;
                return true;
            }
        }

        return isGameEnd;
    }
 public void CheckMissingGameObjectInPlayerList()
    {
        tpsControllers.RemoveAll(item => item == null);
    }
    public void SetUp()
    {
        cac = className.text;
        skill = skillName.text;
        colorIn = int.Parse(colorName.text);
    }
}
