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
    public AllClass[] playerClasses;
    public Color[] playerColor;
    public Items[] items;
    [Serializable]
    public class Skills
    {
        public string skillName;
        public Sprite skillIcon;
        public float skillCooldown;
        public SkillBase skillObj;
    }
    [Serializable]
    public class AllClass
    {
        public string className;
        public float classDamage;
    }
    [Serializable]
    public class Items
    {
        public string itemName;
        public Sprite itemSprite;
        public string itemDescription;
    }
    public override void InstallBindings()
    {
        Container.BindInstance(skills);
        Container.BindInstance(playerStats);
        Container.BindInstance(playerClasses);
        Container.BindInstance(playerController);
        Container.BindInstance(playerColor);
        Container.BindInstance(items);
    }
}