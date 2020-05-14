using UnityEngine;
using Zenject;
using UnityEngine.UI;
public class DefaultInstaller : MonoInstaller
{
    [Inject]
    PlayerStat[] CharacterStats;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIManager>().AsSingle();
    }
    [System.Serializable]
    public class PlayerStat
    {
        public Sprite iconRace;
        public string characterRace;
        public float currentHealth;
        public float maxHealth;
        public float regenHealth;

        public float stamina;
        public float maxStamina;
        public float regenStamina;

        public float mana;
        public float maxMana;
        public float regenMana;

        public float playerDamage;
        public float attackUseStamina;
    }
}