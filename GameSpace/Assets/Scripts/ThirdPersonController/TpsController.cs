using Photon.Pun;
using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(CharacterController))]
public class TpsController : MonoBehaviourPunCallbacks,IPunObservable
{
    public bool isProcess;
    public bool isDead;
    public bool isShowModel;
    public int playerTeam;
    public Transform centerTransform;
    public string playerClass;
    public string playerRace;
    public int colorIndex;
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
    public string playerSkillName;

    public SkillBase playerSkill;
    public float cooldownSkill;
    public float maxCooldownSkill;

    [Header("BasicAttack")]
    public DamageBase damageCollider;

    public ProjectileBase magicAttack;
    public ProjectileBase arrowAttack;

    [Header("ControllerSetting")]
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    [Header("PlayerModel")]
    public GameObject knightModel;
    public GameObject mageModel;
    public GameObject archerModel;
    private GameObject model = null;
    private Animator animator;

    private Color[] characterColor;
    private DefaultInstaller.PlayerStat[] refStats;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 rotation = Vector2.zero;
    private GameManager gameManager;
    private UIManager uIManager;
    private float attackCounter;
    private BoxCollider meleeDamageCollider;
    private bool isMoving;
    private bool isMovingFormOtherPV;
    private bool isGameEnd;
    [HideInInspector]
    public bool canMove = true;

    [Header("Potion")]
    public int healCount=1;
    
    public ParticleSystem bloodParticle;

    private float hpRegenCounter = 0;
    private float staminaRegenCounter = 0;
    private float manaRegenCounter = 0;
    public GameObject endGameCanvas;
    public  TextMeshProUGUI teamText;
    private MyGameSettingInstaller.Skills[] refSkill;
    private MyGameSettingInstaller.AllClass[] classes;
    private PhotonView pv;

   
    public void SettingUIManager(UIManager uIManager_)
    {
        uIManager = uIManager_;
    }

    private void Awake()
    {
        health = 50;
        pv = GetComponent<PhotonView>();
        SetPlayerModelInfo();
    }

    private void Start()
    {
        if (isProcess)
        {
            BarsSetting.singleton.player = this;
        }
        if (!isProcess)
        {
            playerCameraParent.gameObject.SetActive(false);
        }
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        meleeDamageCollider = damageCollider.GetComponent<BoxCollider>();
        meleeDamageCollider.enabled = false;

        if (!isShowModel)
        {
            PlayFabController.singleton.tpsControllers.Add(this);
        }

    }
    public void SetUpPlayerOffline(string race, string playerClass, string s, int color, int teamIndex)
    {
        RPC_SetUpPlayerModel(race, playerClass, s, color, teamIndex);
        teamText.gameObject.SetActive(false);
    }
    public void SetUpPlayer(string race,string playerClass,string s,int color,int teamIndex)
    {
     
        pv.RPC("RPC_SetUpPlayerModel", RpcTarget.AllBuffered, race, playerClass, s,color, teamIndex);
    }

    public void SetPlayerModelInfo()
    {
        refStats = PlayFabController.singleton.refStats;
        refSkill = PlayFabController.singleton.refSkill;
        characterColor = PlayFabController.singleton.characterColor;
        classes = PlayFabController.singleton.classes;
    }

    [PunRPC]
    public void RPC_SetUpPlayerModel(string race,string playerClass, string s, int color,int teamIndex)
    {
        SetRace(race);
        SetClass(playerClass);
        SetSkill(s);
        SetColor(color);
        if (PlayFabController.singleton.gameState == GameState.Team)
        {
            playerTeam = teamIndex;
            if (pv.IsMine)
            {
                teamText.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
            }
            teamText.text = "Team " + playerTeam;
        }
        else
        {
            teamText.gameObject.SetActive(false);
        }
     
    }

    public void SetSkill(string skillName)
    {
        for (int i = 0; i < refSkill.Length; i++)
        {
            if (refSkill[i].skillName == skillName)
            {
                playerSkill = refSkill[i].skillObj;
                playerSkillName = refSkill[i].skillName;
                maxCooldownSkill = refSkill[i].skillCooldown;
                cooldownSkill = maxCooldownSkill;
                if (uIManager != null)
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
                playerRace = r;
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
                if (uIManager != null)
                    uIManager.InitSetRace(refStats[i].iconRace, refStats[i].characterRace);
                
            }
        }
    }

    public void SetClass(string playerClass_)
    {
        playerClass = playerClass_;
        print(playerClass);
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
            else if (playerClass_ == "Archer")
            {
                model = archerModel;
                model.SetActive(true);
            }
        }

        colorIndex = PlayFabController.singleton.playerColorIndex;
        model.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", characterColor[colorIndex]);
        animator = model.GetComponent<Animator>();
    }

    public void SetColor(int colorIndex_)
    {
        colorIndex = colorIndex_;
        model.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", characterColor[colorIndex]);
    }

    private void Update()
    {

       
            if (Input.GetKeyUp(KeyCode.Escape))
            {
            SwitchLevel();
            }
        
      
       
       if(PlayFabController.singleton.isGameEnd)
        {
            if (pv.IsMine)
            {
                if (isGameEnd == false)
                {
                    
                    CreateEndGameCanvas();
                    isGameEnd = true;
                }
            }
          
        }


        if (isDead)
        {
            health = 0;
            GetComponent<CharacterController>().enabled = false;
            knightModel.SetActive(false);
            archerModel.SetActive(false);
            mageModel.SetActive(false);
            return;
        }
        if (!isProcess)
        {
            if(animator!=null)
            animator.SetBool("Moving", isMovingFormOtherPV);
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
    
        if (isProcess)
        {
            if (cooldownSkill <= maxCooldownSkill)
            {
                cooldownSkill += Time.deltaTime;
            }
            else
            {
                cooldownSkill = maxCooldownSkill;
            }
            if (uIManager != null)
            {
                uIManager.cooldownTime = 1 - (cooldownSkill / maxCooldownSkill);
            }

            if (pv.IsMine)
            {
                if (health < maxHealth)
                {
                    hpRegenCounter += Time.deltaTime;
                    if (hpRegenCounter > 1)
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
                    if (manaRegenCounter > 1)
                    {
                        mana += regenMana;
                        manaRegenCounter = 0;
                    }
                }
                else
                {
                    mana = maxMana;
                }
            }
         
        }
    }

    private void Movement()
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
                    pv.RPC("RPC_BasicAttackTrigger", RpcTarget.AllBuffered);
                    stamina -= attackUseStamina;
                    pv.RPC("RPC_OnCallBasicAttack", RpcTarget.AllBuffered);

                    canAttack = false;
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                if (playerSkill.useMana <= mana)
                {
                    if (cooldownSkill == maxCooldownSkill)
                    {
                        pv.RPC("RPC_BasicAttackTrigger", RpcTarget.AllBuffered);
                        mana -= playerSkill.useMana;
                        canAttack = false;
                        pv.RPC("RPC_OnCallSkill", RpcTarget.AllBuffered);

                        cooldownSkill = 0;
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                if (healCount > 0)
                {
                    if (health < maxHealth)
                    {
                        pv.RPC("RPC_HealPotion", RpcTarget.AllBuffered);
                        healCount--;
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
        if (pv.IsMine)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                isMoving = true;
            }
            else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                isMoving = false;

            }
            animator.SetBool("Moving", isMoving);
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

    public void DoDamgeAction(float damage)
    {
        
        pv.RPC("RPC_CalculateDoDamge", RpcTarget.AllBuffered, damage);
    }

    [PunRPC]
    public void RPC_HealPotion()
    {
        this.health += PlayFabController.singleton.GetHealValue();
        if (this.health > this.maxHealth)
        {
            this.health = this.maxHealth;
        }
    }
    [PunRPC]
    public void RPC_CalculateDoDamge(float d)
    {
        if (!bloodParticle.isPlaying)
        {
            bloodParticle.Play();
        }
        health -= d;

        pv.RPC("RPC_CheckIfPlayerIsDead", RpcTarget.AllBuffered);
        if (health < 0)
        {
           
     
            health = 0;
        }
    }

    [PunRPC]
    public void RPC_BasicAttackTrigger()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack1Trigger");
        }
    }

  
    [PunRPC]
    public void RPC_OnCallBasicAttack()
    {
        Vector3 v = GetCenterTransform().position + GetCenterTransform().forward;
        if (playerClass == "Warrior")
        {
            Invoke("OpenDamageCollider", 0.2f);
            Invoke("CloseDamageCollider", 0.26f);
            damageCollider.damage = playerDamage;
        }
        else if (playerClass == "Mage")
        {


            ProjectileBase magic = Instantiate(magicAttack,v, GetCenterTransform().rotation);
                magic.SetUpOwner(this);
                magic.damage = playerDamage;
            

        }
        else if (playerClass == "Archer")
        {
            
                ProjectileBase arrow = Instantiate(arrowAttack, v, GetCenterTransform().rotation);
            arrow.SetUpOwner(this);
                arrow.damage = playerDamage;
            
           
        }
    }

    [PunRPC]
    public void RPC_OnCallSkill()
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
    [PunRPC]
    public void RPC_CheckIfPlayerIsDead()
    {
        if (health <= 0)
        {
            isDead = true;
            if (PlayFabController.singleton.tpsControllers.Contains(this))
            {
                PlayFabController.singleton.tpsControllers.Remove(this);
            }

        }
     
        if (PlayFabController.singleton.CheckPlayerAlive(PhotonNetwork.PlayerList.Length))
        {
            PlayFabController.singleton.isGameEnd = true;
        }
    }
    void CreateEndGameCanvas()
    {

        GameObject o = Instantiate(endGameCanvas);

        if (PlayFabController.singleton.gameState == GameState.LastManStanding)
        {
            if (this.isDead == true)
            {

                o.transform.GetChild(1).gameObject.SetActive(true);//lose
            }
            else
            {
                o.transform.GetChild(0).gameObject.SetActive(true);//win
            }
        }
        else
        {
            if(playerTeam== PlayFabController.singleton.teamWin)
            {
                o.transform.GetChild(0).gameObject.SetActive(true);//win
            }
            else
            {
                o.transform.GetChild(1).gameObject.SetActive(true);//lose
            }
        }
      
        Invoke("SwitchLevel",4);
    }
    
    public void SwitchLevel()
    {
        if (!PhotonNetwork.InRoom)
        {
            return;
        }
        isGameEnd = true;
        StartCoroutine(DoSwitchLevel());
    }
    IEnumerator DoSwitchLevel()
    {
        if (!PhotonNetwork.InRoom)
        {
            yield return null;
        }

        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        PhotonNetwork.LoadLevel(1);
        PlayFabController.singleton.isGameEnd = false;
        //SceneManager.LoadScene(1);
    }
    IEnumerator OnPlayerLeave()
    {
        
        pv.RPC("RPC_OnPlayerLeave", RpcTarget.AllBuffered);
        yield return new WaitForEndOfFrame();

    }
    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        PlayFabController.singleton.CheckMissingGameObjectInPlayerList();
    }
    void OnApplicationQuit()
    {
        pv.RPC("RPC_OnPlayerLeave", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void RPC_OnPlayerLeave()
    {
        PlayFabController.singleton.CheckMissingGameObjectInPlayerList();
    }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isMoving);
            stream.SendNext(isDead);
         
        }
        else
        {
            isMovingFormOtherPV = (bool)stream.ReceiveNext();
            isDead = (bool)stream.ReceiveNext();
        }
    }
}