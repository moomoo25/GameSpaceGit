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
    public PlayerStat playerStat;
    public SkillBase skill;
    [Header("ControllerSetting")]
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    public Animator animator;
    
 
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 rotation = Vector2.zero;
    private UIManager uIManager;
    private float attackCounter;
    [HideInInspector]
    public bool canMove = true;
    public bool canAttack = true;

    float hpRegenCounter = 0;
    float staminaRegenCounter = 0;
    float manaRegenCounter = 0;
    [Inject]
    public void SettingUIManager(UIManager uIManager_)
    {
        uIManager = uIManager_;
    }
    void Start()
    {
        if (!isProcess)
        {
            this.enabled = false;
        }
        uIManager.SetPlayerStat(this.playerStat); // UpdatePlayerStat
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
    }

    void Update()
    {

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


        if(playerStat.currentHealth < playerStat.maxHealth)
        {
            hpRegenCounter += Time.deltaTime;
            if(hpRegenCounter > 1)
            {
                playerStat.currentHealth += playerStat.regenHealth;
                hpRegenCounter = 0;
            }
        }
        else
        {
            playerStat.currentHealth = playerStat.maxHealth;
        }


        if (playerStat.stamina < playerStat.maxStamina)
        {
            staminaRegenCounter += Time.deltaTime;
            if (staminaRegenCounter > 1)
            {
                playerStat.stamina+= playerStat.regenStamina;
                staminaRegenCounter = 0;
            }
        }
        else
        {
            playerStat.stamina = playerStat.maxStamina;
        }
        if (playerStat.mana < playerStat.maxMana)
        {
            manaRegenCounter += Time.deltaTime;
            if (manaRegenCounter >1 )
            {
                playerStat.mana+= playerStat.regenMana;
                manaRegenCounter = 0;
            }
        }
        else
        {
            playerStat.mana = playerStat.maxMana;
        }

        uIManager.SetPlayerStat(this.playerStat); // UpdatePlayerStat
    }

    void Movement()
    {
        if (canAttack == false || isDead)
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
                if (playerStat.attackUseStamina <= playerStat.stamina)
                {
                    animator.SetTrigger("Attack1Trigger");
                    playerStat.stamina -= playerStat.attackUseStamina;
                    canAttack = false;
                }

            }
            if (Input.GetMouseButtonUp(1))
            {
                if (playerStat.skillUseMana <= playerStat.mana)
                {
                    animator.SetTrigger("Attack1Trigger");
                    playerStat.mana -= playerStat.skillUseMana;
                    canAttack = false;
                    OnCallSkill();
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
    public Transform GetCenterTransform()
    {
        return centerTransform;
    }
    public void OnCallSkill()
    {
        skill.OnSkillAction(this);
    }
    public void CheckIfPlayerIsDead()
    {
        if (playerStat.currentHealth <= 0)
        {
            isDead = true;
        }
    }
}

[System.Serializable]
public class PlayerStat
{
    public string race;

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
    public float skillUseMana;
}
