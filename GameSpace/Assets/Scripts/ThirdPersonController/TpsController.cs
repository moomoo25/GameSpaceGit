using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
[RequireComponent(typeof(CharacterController))]
public class TpsController : MonoBehaviour
{
    public bool isProcess;
    public bool isDead;
    public Transform centerTransform;
    public string playerClass;
    public string playerRace;
    public float health;
    public float maxHealth;
    private float regenHealth;
    public float stamina;
    public float maxStamina;
    private float regenStamina;
    public float mana;
    public float maxMana;
    private float regenMana;
    public float playerDamage;
    private float attackUseStamina;
    public bool canAttack = true;
    [Header("Skill")]
    public SkillBase playerSkill;
    public float cooldownSkill;
    public float maxCooldownSkill;
    [Header("BasicAttack")]
    public DamageBase damageCollider;
    public DamageBase magicAttack;
    public DamageBase arrowAttack;
    [Header("ControllerSetting")]
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    public GameObject knightModel;
    public GameObject mageModel;
    public GameObject archerModel;
    private GameObject model = null;
    private Animator animator;

    public Color[] characterColor;
    private DefaultInstaller.PlayerStat[] refStats;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 rotation = Vector2.zero;
    private UIManager uIManager;
    private float attackCounter;
    private BoxCollider meleeDamageCollider;

    [HideInInspector]
    public bool canMove = true;


    public ParticleSystem bloodParticle;

    float hpRegenCounter = 0;
    float staminaRegenCounter = 0;
    float manaRegenCounter = 0;
    private MyGameSettingInstaller.Skills[] refSkill;
    private MyGameSettingInstaller.AllClass[] classes;
    [Inject]
    public void SettingUIManager(UIManager uIManager_)
    {
        uIManager = uIManager_;
    }
    [Inject]
    public void SettingPlayerStat(DefaultInstaller.PlayerStat[] playerStats_)
    {
        refStats = playerStats_; // can use 
    }
    [Inject]
    public void SetUpSkillAndClass(MyGameSettingInstaller.Skills[] refSkills, MyGameSettingInstaller.AllClass[] c)
    {
        refSkill = refSkills; // can use 
        classes = c;
    }
    [Inject]
    public void SetupColor(Color[] c)
    {
        characterColor = c;
    }
    private void Awake()
    {
    
        SetRace("Human"); 
        // SetClass();
       // SetSkill("Laser Beam");
    }
    void Start()
    {
        if (isProcess)
        {
            BarsSetting.singleton.player = this;
        }
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        meleeDamageCollider = damageCollider.GetComponent<BoxCollider>();
        meleeDamageCollider.enabled = false;
   
    }
    public void SetSkill(string skillName)
    {
        for (int i = 0; i < refSkill.Length; i++)
        {
            if(refSkill[i].skillName == skillName)
            {
                playerSkill = refSkill[i].skillObj;
                maxCooldownSkill = refSkill[i].skillCooldown;
                cooldownSkill = maxCooldownSkill;
                uIManager.SetPlayerSkill(refSkill[i]);
            }
        }
    }
    public void SetRace(string r)
    {
        for (int i = 0; i < refStats.Length; i++)
        {
            if (refStats[i].characterRace == r)
            {
                health = refStats[i].currentHealth;
                maxHealth = health;
                regenHealth = refStats[i].regenHealth;
                stamina = refStats[i].stamina;
                maxStamina = refStats[i].maxStamina;
                regenStamina = refStats[i].regenStamina;
                mana = refStats[i].mana;
                maxMana = mana;
                regenMana = refStats[i].regenMana;
                attackUseStamina = refStats[i].attackUseStamina;
                uIManager.InitSetRace(refStats[i].iconRace, refStats[i].characterRace);
             
            }
        
        }
    }
    public void SetClass(string playerClass_)
    {
   
        knightModel.SetActive(false);
        archerModel.SetActive(false);
        mageModel.SetActive(false);
  
        for (int i = 0; i < classes.Length; i++)
        {
     
            if (playerClass_ == classes[i].className)
            {
                playerDamage = classes[i].classDamage;
            }
             if (playerClass_ == "Warrior")
            {
                model = knightModel;
                model.SetActive(true);

            }
            else if (playerClass_ == "Mage")
            {
                model = mageModel;
                model.SetActive(true);

            }
            else if(playerClass_ == "Archer")
            {
                model = archerModel;
                model.SetActive(true);
            }
        }
  
   
        animator = model.GetComponent<Animator>();
    }
    public void SetColor(int colorIndex)
    {
        model.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", characterColor[colorIndex]);
    }
    void Update()
    {
        
        if (animator != null)
        {
            CheckIfPlayerIsDead();
        }

        if (isDead)
        {
            GetComponent<CharacterController>().enabled = false;
            knightModel.SetActive(false);
            archerModel.SetActive(false);
            mageModel.SetActive(false);
            return;
        }
        if (!isProcess)
        {
            return;
        }
        if (canAttack == false)
        {
            attackCounter += Time.deltaTime;
            if (attackCounter > 0.75f)
            {
                canAttack = true;
                attackCounter = 0;
            }
        }

        Movement();

        if(cooldownSkill<= maxCooldownSkill)
        {
            cooldownSkill += Time.deltaTime;
          
        }
        else
        {
            cooldownSkill = maxCooldownSkill;
        }
        uIManager.cooldownTime = 1 - (cooldownSkill / maxCooldownSkill);
        if(health < maxHealth)
        {
            hpRegenCounter += Time.deltaTime;
            if(hpRegenCounter > 1)
            {
                health += regenHealth;
                hpRegenCounter = 0;
            }
        }
        else
        {
            health = maxHealth;
        }


        if (stamina < maxStamina)
        {
            staminaRegenCounter += Time.deltaTime;
            if (staminaRegenCounter > 1)
            {
                stamina += regenStamina;
                staminaRegenCounter = 0;
            }
        }
        else
        {
             stamina = maxStamina;
        }
        if (mana < maxMana)
        {
            manaRegenCounter += Time.deltaTime;
            if (manaRegenCounter >1 )
            {
                mana+= regenMana;
                manaRegenCounter = 0;
            }
        }
        else
        {
             mana = maxMana;
        }


  
    }

    void Movement()
    {
        if (canAttack == false || isDead || !isProcess)
        {
            return;
        }

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate move direction based on axes

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetMouseButtonUp(0))
            {
                if (attackUseStamina <= stamina)
                {
                    animator.SetTrigger("Attack1Trigger");
                    stamina -= attackUseStamina;
                    Vector3 v = GetCenterTransform().position + GetCenterTransform().forward;
                    if (playerClass == "Warrior")
                    {
                        Invoke("OpenDamageCollider", 0.2f);
                        Invoke("CloseDamageCollider", 0.26f);
                        damageCollider.damage = playerDamage;
                    }
                    else if (playerClass == "Mage")
                    {
                        DamageBase magic = Instantiate(magicAttack, v, GetCenterTransform().rotation);
                        magic.SetUpOwner(this);
                        magic.damage = playerDamage;
                    }
                    else if (playerClass == "Archer")
                    {
                        DamageBase arrow = Instantiate(arrowAttack, v, GetCenterTransform().rotation);
                        arrow.SetUpOwner(this);
                        arrow.damage = playerDamage;
                    }
                    canAttack = false;
                }

            }
            if (Input.GetMouseButtonUp(1))
            {
              
                if (playerSkill.useMana <=mana)
                {
                    if (cooldownSkill == maxCooldownSkill)
                    {
                        animator.SetTrigger("Attack1Trigger");
                        mana -= playerSkill.useMana;
                        canAttack = false;
                        OnCallSkill();
                        cooldownSkill = 0;
                    }
                  
                }

            }

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
  
        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }
    }
    public void OpenDamageCollider()
    {
        meleeDamageCollider.enabled = true;
    }
    public void CloseDamageCollider()
    {
        meleeDamageCollider.enabled = false;
    }
    public Transform GetCenterTransform()
    {
        return centerTransform;
    }
    public void OnCallSkill()
    {
        playerSkill.OnSkillAction(this);
    }
    public void PlayBloodEffect()
    {
        if (!bloodParticle.isPlaying)
        {
            bloodParticle.Play();
        }
    }
    public void CheckIfPlayerIsDead()
    {
        if (health <= 0)
        {
            isDead = true;
        }
    }
}


