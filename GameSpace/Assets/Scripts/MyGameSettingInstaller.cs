using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using System;
//[CreateAssetMenu(fileName = "MyGameSettingInstaller", menuName = "Installers/MyGameSettingInstaller")]
public class MyGameSettingInstaller : ScriptableObjectInstaller<MyGameSettingInstaller>
{
    public GameObject playerController;
    public DefaultInstaller.PlayerStat[] playerStats;
    public Skills[] skills;
    public Races[] races;
    [Serializable]
    public class Skills
    {
        public SkillBase skillObj;
        public string skillName;
    }
    [Serializable]
    public class Races
    {
        public string racesName;
        public Sprite racesImage;
    }
    public override void InstallBindings()
    {
        Container.BindInstance(skills);
        Container.BindInstance(races);
        Container.BindInstance(playerStats);
        Container.BindInstance(playerController);
    }
}