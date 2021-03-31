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
    //[SerializeField] private bool isLanded = false;
    [SerializeField] private bool isRunning = false;
    //[SerializeField] private bool flipped = false;
    [SerializeField] private bool inPortalRange = false;
    [SerializeField] private bool timeCheat = false;
    [SerializeField] private bool canPhase = false;

    //[Header("UI ELEMENTS")]
    Slider rechargeOne;
    Slider rechargeTwo;
    Text scoreText;
    Text itemTotal;
    Text Dialog;
    RawImage chargeOne;
    RawImage chargeTwo;
    RawImage worldSwapUI;
    float itemCount;
    float scoreTime;
    float timer;

    [Header("MOVEMENT")]
    [SerializeField] private float playerSpeed = 10.0f;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private float gravityValue = -30f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private Vector3 playerVelocity;
    private Vector3 teleport;

    [Header("TIME SLOW")]
    [SerializeField] private float slowdownTimer = 2f;
    [SerializeField] private float slowdownFactor = 0.05f;
    [SerializeField] private float timeReset = 2f;
    [SerializeField] private float maxChargeValue = 10;
    float timeWarpCountdown = 0.0f;

    [Header("RIFTING")]
    [SerializeField] private bool crossed = false;
    private bool sendOff;                                           //teleports the player to another world


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
        if (sendOff == true)
        {
            if (inPortalRange == true)
            {
                TeleportPlayer();
            }
            else
                sendOff = false;
        }
        //ChargeTimePower();
    }

    void Update()
    {
        HandleTimeEvents();
        ChargeTimePower();

        if (controller.isGrounded == true)
        {
            isJumping = false;
        }

        Move();

        if (actionOne.action.triggered && timeCheat == false)
        {
            TimePower();
        }

        if (actionTwo.action.triggered)
        {
            PhasePower();
        }

        if (actionThree.action.triggered && inPortalRange == true)
        {
            WorldSwapPower();
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
                Debug.Log(timeWarpCountdown);
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
        controller.Move(move * Time.unscaledDeltaTime * playerSpeed);

        if (isJumping == false)
        {
            if (groundedPlayer == false)
            {
                playerAnim.StartFreefall();  
                isFalling = true;
            }
            if (groundedPlayer == true)
            {
                if (isFalling == true)
                {
                    playerAnim.JumpSquat();
                    isFalling = false;
                }

                if(move.x == 0 && move.z == 0 && isFalling == false)
                {
                    isRunning = false;
;                }
                if (move.x != 0 && move.z != 0 && isFalling == false)
                {
                    isRunning = true;
                    playerAnim.IsRunning();
                }
                else if (isRunning == false)
                {
                    playerAnim.NotRunning();
                }
            }
            else
                playerAnim.IsRunning();
        }
        else if (isJumping == true && groundedPlayer == false)
        {
            playerAnim.StartFreefall();
            isFalling = true;
        }
        //TODO FIX THIS, ALSO FINISH JUMPSTART
        else if (isFalling == true && groundedPlayer == true)
        {
            playerAnim.JumpSquat();
        }

        // changes the height position of the player
        if (jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            isJumping = true;
            playerAnim.JumpStart();
        }

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
            chargeOne.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        rechargeOne.value = rechargeOne.value + Time.unscaledDeltaTime;


        if (rechargeTwo.value == maxChargeValue)
        {
            chargeTwo.color = new Color(1.0f, 1.0f, 1.0f);
        }
        else
            chargeTwo.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
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

    private void OnTriggerEnter(Collider other)
    {
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

        if (other.tag.Equals("Portal"))
        {
            inPortalRange = true;
            worldSwapUI.enabled = true;
            GameObject portalEntrance = other.gameObject;
            GameObject PortalExit = portalEntrance.transform.parent.GetChild(0).gameObject;

            Dialog.text = "press E to shift to another world";
            crossed = true;
            teleport = PortalExit.transform.position;
        }

        if (other.tag.Equals("Collectible"))
        {
            Destroy(other.gameObject);
            if (itemCount == 4)
            {
                SceneManager.LoadScene("WinScreen");
            }

            itemCount++;
            itemTotal.text = itemCount.ToString();
        }

        if (other.tag.Equals("Enemy"))
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        worldSwapUI.enabled = false;
        inPortalRange = false;
        Dialog.text = "";
    }

    private void TeleportPlayer()
    {
        if (crossed == true)
        {
            player.transform.localPosition = teleport;
            crossed = false;
        }
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

<<<<<<< HEAD
    //calculates and sets time for player UI
    private void TimeTracker()
    {
        timer += Time.deltaTime;
        if (timeCheat == true)
        {
            SlowMo();
        }
            //ResumeTime();
        scoreTime = timer - timer % 1;
        scoreText.text = scoreTime.ToString();
        //Debug.Log(timer - timer % 1);
        
    }

    private void ResumeTime()
    {
        Time.timeScale += (1f / slowdownTimer) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        if (Time.timeScale != 1)
        {
            timeCheat = false;
        }
    }

    private void SlowMo()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void ChargeTimePower()
    {
        rechargeOne.maxValue = 1000;
        rechargeOne.minValue = 0;
        if (rechargeOne.value == 1000)
        {
            chargeOne.color = new Color(1.0f, 1.0f, 1.0f);
        }
        else
            chargeOne.color = new Color(1.0f, 1.0f, 1.0f, 0.40f);
        rechargeOne.value += 1 + Time.unscaledDeltaTime;

        rechargeTwo.maxValue = 1000;
        rechargeTwo.minValue = 0;
        if (rechargeTwo.value == 1000)
        {
            chargeTwo.color = new Color(1.0f, 1.0f, 1.0f);
        }
        else
            chargeTwo.color = new Color(1.0f, 1.0f, 1.0f, 0.40f);
        rechargeTwo.value += 1 + Time.unscaledDeltaTime;
    }

    IEnumerator TimeSlow()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("timer: " + totalTime );
    }
=======
>>>>>>> f5049b33362a0ea4cb3a2cdb232c848351dee419

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

