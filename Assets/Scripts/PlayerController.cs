using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputActionReference))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference actionOne;
    [SerializeField] private InputActionReference actionTwo;
    [SerializeField] private InputActionReference jumpControl;
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerAnimation playerAnim;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private bool timeCheat = false;
    [SerializeField] private bool canPhase = false;
    [SerializeField] private float playerSpeed = 10.0f;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private float gravityValue = -30f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private Canvas UIdisplay;
    public Transform detectPlayerSphere;
    public float radius = 5f;

    private Vector3 teleport;
    private bool crossed = false;
    public GameObject player;
    Transform cameraMainTransform;
    float slowdownFactor = 0.05f;


    //DELEEEEET
    float totalTime;
    float scoreTime;
    float timer;

    //Canvas UI
    Slider recharge;
    Text scoreText;
    Text itemTotal;
    Image chargeOne;
    Image chargeTwo;
    float itemCount;


    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        playerAnim = gameObject.GetComponent<PlayerAnimation>();
        cameraMainTransform = Camera.main.transform;

        //Canvas UI
        UIdisplay = (Canvas)FindObjectOfType(typeof(Canvas));
        scoreText = GameObject.Find("Canvas/Text").GetComponent<Text>();
        recharge = GameObject.Find("Canvas/TimeDilation Slider").GetComponent<Slider>();
        chargeOne = GameObject.Find("Canvas/Charge One").GetComponent<Image>();
        chargeTwo = GameObject.Find("Canvas/Charge Two").GetComponent<Image>();
        itemTotal = GameObject.Find("Canvas/Item Total").GetComponent<Text>();

        //Canvas UI set value
        recharge.value = 0.0f;
    }

    private void Start()
    {
        LockMouse();
        Physics.IgnoreLayerCollision(0, 9);
        Physics.GetIgnoreLayerCollision(8, 10);
    }

    private void FixedUpdate()
    {
        TimeTracker();
        TeleportPlayer();
        recharge.maxValue = 1000;
        recharge.minValue = 0;

        if(recharge.value >= 500)
        {
            chargeOne.color = new Color (0.0f, 0.7f, 0.0f);
        }
        if(recharge.value == 1000)
        {
            chargeTwo.color = new Color (0.0f, 0.7f, 0.0f);
        }

        recharge.value += 1 + Time.unscaledDeltaTime;


    }

    void Update()
    {
        Move();
        if (actionOne.action.triggered)
        {
            TimePower();
        }

        if (actionTwo.action.triggered)
        {
            PhasePower();
        }
        //Debug.Log(Time.timeScale);
    }

    private void Move()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0;
        controller.Move(move * Time.unscaledDeltaTime * playerSpeed);
        
        // it jumps
        // changes the height position of the player..
        if (jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            playerAnim.InAir();
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

    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void TimePower()
    {
        timeCheat = !timeCheat;
    }

    private void PhasePower()
    {
        canPhase = !canPhase;
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

        if (other.tag.Equals("Collectible"))
        {
            Destroy(other.gameObject);
            itemCount++;
            itemTotal.text = itemCount.ToString();
        }
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
        jumpControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        actionOne.action.Disable();
        actionTwo.action.Disable();
        jumpControl.action.Disable();
    }
    
    //calculates and sets time for player UI
    private void TimeTracker()
    {
        timer += Time.deltaTime;
        if (timeCheat == true)
        {
            SlowMo();
        }
        else
            ResumeTime();
        scoreTime = timer - timer % 1;
        scoreText.text = scoreTime.ToString();
        //Debug.Log(timer - timer % 1);
        
    }

    private void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    private void SlowMo()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    IEnumerator TimeSlow()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("timer: " + totalTime );
    }

    /// <summary>
    /// Loads the information from previous levels into the canvas.
    /// </summary>
    private void LoadCanvasInformation()
    {

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

