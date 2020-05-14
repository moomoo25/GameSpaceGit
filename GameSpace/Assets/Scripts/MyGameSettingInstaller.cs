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
    [Serializable]
    public class Skills
    {
        public SkillBase skillObj;
        public string skillName;
    }

    public override void InstallBindings()
    {
        Container.BindInstance(skills);
        Container.BindInstance(playerStats);
        Container.BindInstance(playerController);
    }
}