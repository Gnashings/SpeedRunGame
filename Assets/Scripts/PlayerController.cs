using System.Security.AccessControl;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Slider = UnityEngine.UI.Slider;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputActionReference))]
public class PlayerController : MonoBehaviour
{
    #pragma warning disable 0649
    [Header("PLAYER INPUT")]
    [SerializeField] public GameObject player;
    public InputActionReference movementControl;

    [SerializeField] public InputActionReference actionOne;
    [SerializeField] public InputActionReference actionTwo;
    [SerializeField] public InputActionReference actionThree;
    [SerializeField] public InputActionReference actionFour;
    [SerializeField] public InputActionReference jumpControl;
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private PlayerAnimation playerAnim;
    
    Transform cameraMainTransform;
    [HideInInspector] public Transform detectPlayerSphere;
    public float radius = 5f;                                           //radius of detection

    [Header("STATES")]
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isFalling = false;
    [SerializeField] private bool isRunning = false;
    [SerializeField] public bool slowed = false;
    [SerializeField] private bool inSwitchRange = false;
    [SerializeField] private bool timeCheat = false;
    [SerializeField] public bool canPhase = false;
    [SerializeField] public bool hitStun = false;
    [HideInInspector] public bool reaching = false;
    [HideInInspector] public bool paperReaching = false;

#pragma warning restore 0649
    //[Header("UI ELEMENTS")]
    Slider healthbar;
    Slider rechargeOne;
    Slider rechargeTwo;
    Text scoreText;
    Text itemTotal;
    Text Dialog;
    Text hpText;
    RawImage chargeOne;
    RawImage chargeTwo;
    RawImage worldSwapUI;
    RawImage frozenScreen;
    RawImage gasScreen;
    RawImage altScreen;
    RawImage paper;
    RawImage page1;
    RawImage page2;
    RawImage page3;
    public int itemCount;
    public int switchCount;
    public int objCount;
    float scoreTime;
    float timer;

    [Header("MOVEMENT")]
    [SerializeField] private float playerSpeed = 10.0f;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private float gravityValue = -30f;
    [SerializeField] public float rotationSpeed = 20f;
    [SerializeField] private Vector3 playerVelocity;
    [Tooltip("Divided by this value.")]
    [SerializeField] private float slowBy = 2f;
    [SerializeField] private float totalTimeSlowed = 3f;
    [SerializeField] public float distanceToGround = 1f;
    private float timeSlowed;
    private Vector3 teleport;
    int GroundLayer;

    [Header("TIME SLOW")]
    [SerializeField] private float slowdownTimer = 2f;
    [SerializeField] private float slowdownFactor = 0.05f;
    [SerializeField] private float timeReset = 2f;
    [SerializeField] private float maxChargeValue = 10;
    float timeWarpCountdown = 0.0f;

    [Header("RIFTING")]
    [SerializeField] public bool crossed = false;
    [SerializeField] private float allowedTimeInRift = 5.0f;

    [Header("DAMAGE VALUES")]
    [SerializeField] float droneDamage;
    [SerializeField] float gasDamage;
    [SerializeField] float riftDamage;
    [SerializeField] public float healAmount;
    [SerializeField] public float HPUpAmount;


    private Vector3 portalEntrancePosition;
    private bool sendOff;                                           //teleports the player to another world
    private float timeInRift;
    private bool kickOut = false;
    bool hitSwitch = false;
    float jumptimer = 0.0f;
    float jumptime = .15f;
    bool dialogOn = false;

    //sound
    public AudioSource healSound;
    public AudioSource stimSound;
    public AudioSource altSound;
    public AudioSource freezeSound;
    public AudioSource jumpSound;
    public AudioSource jump2Sound;
    bool hasPlayed_1 = false;
    bool hasPlayed_2 = false;


    private bool setFirstButton = false;

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        playerAnim = gameObject.GetComponent<PlayerAnimation>();
        playerActions = gameObject.GetComponent<PlayerActions>();
        cameraMainTransform = Camera.main.transform;
        GroundLayer = LayerMask.GetMask("Ground");

        //Canvas UI
        healthbar = GameObject.Find("Canvas/HealthBar").GetComponent<Slider>();
        hpText = GameObject.Find("Canvas/TotalHPText").GetComponent<Text>();
        //scoreText = GameObject.Find("Canvas/Text").GetComponent<Text>();
        rechargeOne = GameObject.Find("Canvas/TimeDilation Slider").GetComponent<Slider>();
        rechargeTwo = GameObject.Find("Canvas/TimeDilation Slider2").GetComponent<Slider>();
        chargeOne = GameObject.Find("Canvas/Charge One").GetComponent<RawImage>();
        chargeTwo = GameObject.Find("Canvas/Charge Two").GetComponent<RawImage>();
        worldSwapUI = GameObject.Find("Canvas/Worldswap UI").GetComponent<RawImage>();
        itemTotal = GameObject.Find("Canvas/Item Total").GetComponent<Text>();
        Dialog = GameObject.Find("Canvas/Dialog").GetComponent<Text>();
        frozenScreen = GameObject.Find("Canvas/FrozenScreen").GetComponent<RawImage>();
        gasScreen = GameObject.Find("Canvas/GasScreen").GetComponent<RawImage>();
        altScreen = GameObject.Find("Canvas/AltWorldScreen").GetComponent<RawImage>();
        paper = GameObject.Find("Canvas/Paper").GetComponent<RawImage>();
        page1 = GameObject.Find("Canvas/Page1").GetComponent<RawImage>();
        page2 = GameObject.Find("Canvas/Page2").GetComponent<RawImage>();
        page3 = GameObject.Find("Canvas/Page3").GetComponent<RawImage>();

        //Canvas UI set value
        rechargeOne.value = 0.0f;
        rechargeOne.minValue = 0;
        rechargeOne.maxValue = maxChargeValue;
        rechargeTwo.value = 0.0f;
        rechargeTwo.minValue = 0;
        rechargeTwo.maxValue = maxChargeValue;
    }

    private void Start()
    {
        LockMouse();
        worldSwapUI.enabled = false;
        Physics.IgnoreLayerCollision(0, 9);
        Physics.GetIgnoreLayerCollision(8, 10);

        healthbar.minValue = 0;
        healthbar.maxValue = 100;
        healthbar.value = 100;

        float hptemp = GetComponent<PlayerStats>().TotalHealth;
        hpText.text = hptemp.ToString();

        healSound = GetComponent<AudioSource>();

        gasScreen.enabled = false;
        altScreen.enabled = false;
        paper.enabled = false;
        page1.enabled = false;
        page2.enabled = false;
        page3.enabled = false;
    }

    private void FixedUpdate()
    {
        //UpdateUITImer();

        if (crossed == true)
        {
            DimensionSwapPower();
        }
        if (slowed == true)
        {
            SlowedTimer();
        }
        if (OnGround())
        {
            isJumping = false;
        }
        UpdateHealthUI();
        if(dialogOn == true)
        {
            DialogBoxClear();
        }
       
    }

    void Update()
    {
        HandleTimeEvents();
        ChargeTimePower();
        if (PauseManager2.paused)
        {
            FreeMouse();
            if (setFirstButton == false)
            {
                //myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("Slider"));
                setFirstButton = true;
            }
            //mouseSens = SensitivitySlider.value;
            return;
        }
        if (PauseManager2.paused == false)
        {
            LockMouse();
            setFirstButton = false;
        }

        if (hitStun == false)
        {
            Move();
        }
        else
            HitStun();
        
        if (actionFour.action.triggered && inSwitchRange && reaching != true)
        {
            reaching = true;
            playerAnim.IsReaching();
        }

        if (inSwitchRange == false)
        {
            reaching = false;
        }

        if (actionThree.action.triggered)
        {
            WorldSwapPower();
        }

        if (actionOne.action.triggered && timeCheat == false)
        {
            TimePower();
        }

        if (actionTwo.action.triggered)
        {
            PhasePower();
        }
        Forces();
        if (!hasPlayed_1)
        {
            jumpSound.Play();
            hasPlayed_1 = true;
        }
        if (hasPlayed_1 && !hasPlayed_2)
        {
            jump2Sound.Play();
            hasPlayed_2 = true;
        }

    }

    //governs the changes in Deltatime
    private void HandleTimeEvents()
    {
        if (timeCheat == true)
        {
            frozenScreen.enabled = true;
            freezeSound.Play();

            SlowDownTime();
            if (timeWarpCountdown <= slowdownTimer)
            {
                timeWarpCountdown += Time.unscaledDeltaTime;
            }
            else
            {
                timeWarpCountdown = 0.0f;
                timeCheat = false;
            }
        }

        if (timeCheat == false)
        {
            frozenScreen.enabled = false;

            ResumeTime();
        }
    }

    //counts down the time player can stay in the rift
    private void CountDownRiftTime()
    {
        if (timeInRift <= allowedTimeInRift)
        {
            timeInRift += Time.deltaTime;
            kickOut = false;
        }
        else
        {
            kickOut = true;
        }
    }

    private void SlowedTimer()
    {
        if(timeSlowed <= totalTimeSlowed)
        {
            timeSlowed += Time.deltaTime;
        }
        else
        {
            timeSlowed = 0;
            slowed = false;
        }
    }

    //sets the players position to the previous gate
    private void KickPlayerFromRift()
    {
        player.transform.localPosition = portalEntrancePosition;
        crossed = false;
        kickOut = false;
        timeInRift = 0.0f;
        Debug.Log("returned");
    }

    private void Move()
    {
        groundedPlayer = OnGround();
        if (groundedPlayer && playerVelocity.y <= 0)
        {
            playerVelocity.y = gravityValue;
        }

        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0;
        if (slowed == true)
        {
            controller.Move(move * Time.unscaledDeltaTime * playerSpeed/slowBy);
        }
        else
            controller.Move(move * Time.unscaledDeltaTime * playerSpeed);
           
        //assuming player fell off a cliff
        if (OnGround() == false)
        {
            jumptimer += Time.deltaTime;

            if(jumptimer >= jumptime)
            {
                isFalling = true;
                playerAnim.NotRunning();
                playerAnim.Freefall();
                jumptimer = 0.0f;
            }
        }
        if (groundedPlayer == true)
        {
            playerAnim.IsOnGround();
            //you hit the ground if you were falling
            if (isFalling == true)
            {
                isFalling = false;
                playerAnim.JumpEnd();
            }
            
            //you are not moving
            if(move.x == 0 && move.z == 0)
            {
                isRunning = false;
                playerAnim.NotRunning();
            }
            //otherwise you are running
            if (move.x != 0 && move.z != 0)
            {
                isRunning = true;
                playerAnim.IsRunning();
            }
            
        }
        
        // start your jump
        if (jumpControl.action.triggered && OnGround() && isJumping == false)
        {
            playerAnim.Freefall();
            Jump();
        }

        // unbinds camera rotations based off movement
        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.unscaledDeltaTime * rotationSpeed);
        }
        
    }

    private void Forces()
    {
        //player gravity
        playerVelocity.y += gravityValue * Time.unscaledDeltaTime;
        controller.Move(playerVelocity * Time.unscaledDeltaTime);
    }

    public void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -5.0f * gravityValue);
        isJumping = true;
        jumpSound.Play();
        jump2Sound.Play();
    }

    private void HitStun()
    {
        //Fix or Remove, it LITERALLY sends the player into Orbit if hit by multiple
        //playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.5f * gravityValue);
        isJumping = true;
        if (OnGround() == true)
        {
            hitStun = false;
        }
    }
    float diaTimer = 0.0f;
    float diaTime = 5.0f;
    private void DialogBoxClear()
    {
        if(dialogOn == true)
        {
            diaTimer += Time.unscaledDeltaTime;
            if(diaTimer >= diaTime)
            {
                dialogOn = false;
                Dialog.text = "";
                diaTimer = 0.0f;
            }
        }
    }

    //calculates and sets time for player UI
    private void UpdateUITImer()
    {
        timer += Time.deltaTime; 
        scoreTime = timer - timer % 1;
        scoreText.text = scoreTime.ToString();
    }

    private void ResumeTime()
    {
        Time.timeScale += (1f / timeReset) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        if (Time.timeScale == 1)
        {
            timeCheat = false;
        }
    }

    private void SlowDownTime()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void ChargeTimePower()
    {

        if (rechargeOne.value == maxChargeValue)
        {
            chargeOne.color = new Color(1.0f, 1.0f, 1.0f);
        }
        else
            chargeOne.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
        rechargeOne.value = rechargeOne.value + Time.unscaledDeltaTime;


        if (rechargeTwo.value == maxChargeValue)
        {
            chargeTwo.color = new Color(1.0f, 1.0f, 1.0f);
        }
        else
            chargeTwo.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
        rechargeTwo.value = rechargeTwo.value + Time.unscaledDeltaTime;
    }

    public void UpdateHealthUI()
    {
        healthbar.value = GetComponent<PlayerStats>().Health;
    }

    //checks to see if the time power is able to be used
    public bool TimePower()
    {
        if (rechargeOne.value == maxChargeValue)
        {
            rechargeOne.value = 0;
            return timeCheat = true;
        }
        else if(rechargeTwo.value == maxChargeValue)
        { 
            rechargeTwo.value = 0;
            return timeCheat = true;
        }
        else
            return timeCheat = false;
    }

    private void Heal()
    {
        healSound.Play();
        
        GetComponent<PlayerStats>().HealPlayer(healAmount);
    }

    private void Stim()
    {
        stimSound.Play();

        GetComponent<PlayerStats>().StimPlayer(HPUpAmount);
        dialogOn = true;
        float hptemp = GetComponent<PlayerStats>().TotalHealth;
        hpText.text = hptemp.ToString();
        Dialog.text = "Your total health increased!";
        healthbar.maxValue += HPUpAmount;
    }

    private void PhasePower()
    {
        canPhase = !canPhase;
    }

    private void WorldSwapPower()
    {
        crossed = !crossed;

        if(crossed == true)
        {
            altScreen.enabled = true;
            altSound.Play();
        }
        else
        {
            altScreen.enabled = false;
        }
    }

    private bool Collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Collectible"))
        {
            Destroy(other.transform.parent.gameObject);
            
            Collected = true;
            if (Collected)
            {
                Debug.Log(LevelStats.Items);
                Collected = false;
                itemTotal.text = LevelStats.Items.ToString();
            }
        }

        if (other.tag.Equals("Checkpoint"))
        {
            Destroy(other.transform.parent.gameObject);
            itemTotal.text = LevelStats.Checkpoints.ToString();
        }

        if (other.gameObject.name.Equals("medkit_anim"))
        {
            Destroy(other.gameObject);
            Heal();
        }

        if (other.gameObject.name.Equals("Serum"))
        {
            Destroy(other.gameObject);
            Stim();
        }

        if (other.tag.Equals("Enemy"))
        {
            Damaged(droneDamage);
            hitStun = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Switch"))
        {
            inSwitchRange = true;
            Dialog.text = "press F to flip switch";
            if (reaching == true && hitSwitch == false)
            {
                other.gameObject.BroadcastMessage("Confirm");
                hitSwitch = true;
            }
        }
        if (other.gameObject.name.Equals("gasplanes"))
        {
            gasScreen.enabled = true;

            Damaged(gasDamage);
        }

        if (other.gameObject.name.Equals("paper"))
        {
            Dialog.text = "press 'F' to interact";

            if (actionFour.action.triggered && paper.enabled == false)
            {
                paper.enabled = true;
            }
        }

        if (other.gameObject.name.Equals("page1"))
        {
            Dialog.text = "press 'F' to interact";

            if (actionFour.action.triggered && page1.enabled == false)
            {
                page1.enabled = true;
            }
        }

        if (other.gameObject.name.Equals("page2"))
        {
            Dialog.text = "press 'F' to interact";

            if (actionFour.action.triggered && page2.enabled == false)
            {
                page2.enabled = true;
            }
        }

        if (other.gameObject.name.Equals("page3"))
        {
            Dialog.text = "press 'F' to interact";

            if (actionFour.action.triggered && page3.enabled == false)
            {
                page3.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        worldSwapUI.enabled = false;
        inSwitchRange = false;
        hitSwitch = false;
        Dialog.text = "";
        paper.enabled = false;
        page1.enabled = false;
        page2.enabled = false;
        page3.enabled = false;
    }

    private void TeleportPlayer()
    {
        if (crossed == true)
        {
            player.transform.localPosition = teleport;
        }
    }

    private void DimensionSwapPower()
    {
        if(GetComponent<PlayerStats>().Health >= 1)
        {
            Damaged(riftDamage);
        }
        else
        {
            //kicks you out
            crossed = false;
            altScreen.enabled = false;
        }

    }



    private void Damaged(float damage)
    {
        GetComponent<PlayerStats>().Health -= damage * Time.fixedDeltaTime;
        if(GetComponent<PlayerStats>().Health <= 0)
        {
            YouDie();
        }    
    }


    public void YouDie()
    {
        SceneManager.LoadScene("LoseScreen");
    }

    private void OnEnable()
    {
        movementControl.action.Enable();
        actionOne.action.Enable();
        actionTwo.action.Enable();
        actionThree.action.Enable();
        actionFour.action.Enable();
        jumpControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        actionOne.action.Disable();
        actionTwo.action.Disable();
        actionThree.action.Disable();
        actionFour.action.Disable();
        jumpControl.action.Disable();
    }

    public void StopReaching()
    {
        playerAnim.NotReaching();
        reaching = false;
    }

    void OnDrawGizmosSelected()
    {
        if (detectPlayerSphere == null)
        {
            detectPlayerSphere = transform;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(detectPlayerSphere.position, radius);
    }

    public bool OnGround()
    {
        bool r;
        r = Physics.CheckSphere(transform.position, distanceToGround, GroundLayer);
        return r;
    }


    private void FreeMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

