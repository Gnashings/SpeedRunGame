using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Runtime.CompilerServices;
using System.Net;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputActionReference))]
public class PlayerController : MonoBehaviour
{
    [Header("PLAYER INPUT")]
    [SerializeField] public GameObject player;
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference actionOne;
    [SerializeField] private InputActionReference actionTwo;
    [SerializeField] private InputActionReference actionThree;
    [SerializeField] private InputActionReference jumpControl;
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerAnimation playerAnim;
    Transform cameraMainTransform;
    public Transform detectPlayerSphere;
    public float radius = 5f;                                           //radius of detection

    [Header("STATES")]
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isFalling = false;
    [SerializeField] private bool isRunning = false;
    [SerializeField] public bool slowed = false;
    [SerializeField] private bool inPortalRange = false;
    [SerializeField] private bool inSwitchRange = false;
    [SerializeField] private bool timeCheat = false;
    [SerializeField] public bool canPhase = false;

    //[Header("UI ELEMENTS")]
    Slider rechargeOne;
    Slider rechargeTwo;
    Text scoreText;
    Text itemTotal;
    Text Dialog;
    RawImage chargeOne;
    RawImage chargeTwo;
    RawImage worldSwapUI;
    public int itemCount;
     public int switchCount;
     public int objCount;
    float scoreTime;
    float timer;

    [Header("MOVEMENT")]
    [SerializeField] private float playerSpeed = 10.0f;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private float gravityValue = -30f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private Vector3 playerVelocity;
    [Tooltip("Divided by this value.")]
    [SerializeField] private float slowBy = 2f;
    [SerializeField] private float totalTimeSlowed = 3f;
    private float timeSlowed;
    private Vector3 teleport;

    [Header("TIME SLOW")]
    [SerializeField] private float slowdownTimer = 2f;
    [SerializeField] private float slowdownFactor = 0.05f;
    [SerializeField] private float timeReset = 2f;
    [SerializeField] private float maxChargeValue = 10;
    float timeWarpCountdown = 0.0f;

    [Header("RIFTING")]
    [SerializeField] private bool crossed = false;
    [SerializeField] private float allowedTimeInRift = 5.0f;

    private Vector3 portalEntrancePosition;
    private bool sendOff;                                           //teleports the player to another world
    private float timeInRift;
    private bool kickOut = false;
    public bool reaching = false;
    bool hitSwitch = false;

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        playerAnim = gameObject.GetComponent<PlayerAnimation>();
        cameraMainTransform = Camera.main.transform;

        //Canvas UI
        scoreText = GameObject.Find("Canvas/Text").GetComponent<Text>();
        rechargeOne = GameObject.Find("Canvas/TimeDilation Slider").GetComponent<Slider>();
        rechargeTwo = GameObject.Find("Canvas/TimeDilation Slider2").GetComponent<Slider>();
        chargeOne = GameObject.Find("Canvas/Charge One").GetComponent<RawImage>();
        chargeTwo = GameObject.Find("Canvas/Charge Two").GetComponent<RawImage>();
        worldSwapUI = GameObject.Find("Canvas/Worldswap UI").GetComponent<RawImage>();
        itemTotal = GameObject.Find("Canvas/Item Total").GetComponent<Text>();
        Dialog = GameObject.Find("Canvas/Dialog").GetComponent<Text>();

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
    }

    private void FixedUpdate()
    {
        UpdateUITImer();
        if (crossed == true)
        {
            CountDownRiftTime();
        }
        if (slowed == true)
        {
            SlowedTimer();
        }
        if(controller.isGrounded)
        {
            isJumping = false;
        }

        if (sendOff == true)
        {
            if (inPortalRange == true)
            {
                TeleportPlayer();
            }
            else
                sendOff = false;
        }
        if (kickOut == true)
        {
            KickPlayerFromRift();
        }

    }

    void Update()
    {
        HandleTimeEvents();
        ChargeTimePower();
        Move();

        if (actionThree.action.triggered && inSwitchRange && reaching != true)
        {
            reaching = true;
            playerAnim.IsReaching();
        }

        if (inSwitchRange == false)
        {
            reaching = false;
        }

        if (actionThree.action.triggered && inPortalRange == true)
        {
            WorldSwapPower();
            crossed = true;
        }

        if (actionOne.action.triggered && timeCheat == false)
        {
            TimePower();
        }

        if (actionTwo.action.triggered)
        {
            PhasePower();
        }

    }

    //governs the changes in Deltatime
    private void HandleTimeEvents()
    {
        if (timeCheat == true)
        {
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
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y <= 0)
        {
            playerVelocity.y = 0f;
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
        if (groundedPlayer == false)
        {
            isFalling = true;
            playerAnim.Freefall();
        }
        if (groundedPlayer == true)
        {
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
        if (jumpControl.action.triggered && groundedPlayer && isJumping == false)
        {
            playerAnim.JumpStart();
        }

        //player gravity
        playerVelocity.y += gravityValue * Time.unscaledDeltaTime;
        controller.Move(playerVelocity * Time.unscaledDeltaTime);

        // unbinds camera rotations based off movement
        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.unscaledDeltaTime * rotationSpeed);
        }
    }

    public void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        isJumping = true;
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

    private void PhasePower()
    {
        canPhase = !canPhase;
    }

    private void WorldSwapPower()
    {
        sendOff = true;
    }

    private void UwU()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        /**
        if (other.tag.Equals("Phase Enter") && canPhase)
        {
            GameObject entrance = other.gameObject;
            GameObject exit = entrance.transform.parent.GetChild(0).gameObject;

            teleport = exit.transform.position;

            Debug.Log("crossed");
            crossed = true;
            //tbr
            //playerSpeed = playerSpeed * 5;
        }
        **/

        if (other.tag.Equals("Portal"))
        {
            inPortalRange = true;
            worldSwapUI.enabled = true;
            GameObject portalEntrance = other.gameObject;
            GameObject PortalExit = portalEntrance.transform.parent.GetChild(0).gameObject;

            Dialog.text = "press E to shift to another world";
            teleport = PortalExit.transform.position;
            portalEntrancePosition = portalEntrance.transform.position;
        }

        if (other.tag.Equals("Collectible"))
        {
            other.gameObject.BroadcastMessage("CountUp");
            Destroy(other.gameObject);
            itemCount++;
            itemTotal.text = itemCount.ToString();
        }

        if (other.tag.Equals("Enemy"))
        {
            YouDie();
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Switch"))
        {
            inSwitchRange = true;
            Dialog.text = "press E to shift to flip switch";
            if (reaching == true && hitSwitch == false)
            {
                other.gameObject.BroadcastMessage("Confirm");
                hitSwitch = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        worldSwapUI.enabled = false;
        inPortalRange = false;
        inSwitchRange = false;
        hitSwitch = false;
        Dialog.text = "";
    }

    private void TeleportPlayer()
    {
        if (crossed == true)
        {
            player.transform.localPosition = teleport;
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
        jumpControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        actionOne.action.Disable();
        actionTwo.action.Disable();
        actionThree.action.Disable();
        jumpControl.action.Disable();
    }


    /// <summary>
    /// Loads the information from previous levels into the canvas.
    /// </summary>
    private void LoadCanvasInformation()
    {

    }
    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

}

