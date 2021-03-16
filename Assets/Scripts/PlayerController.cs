using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference ActionOne;
    [SerializeField] private InputActionReference jumpControl;
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject poggers;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 10.0f;
    [SerializeField] private float jumpHeight = 10.0f;
    [SerializeField] private float gravityValue = -9.81f;
    
    [SerializeField] private float rotationSpeed = 4f;
    private Vector3 teleport;
    public bool crossed = false;
    public GameObject player;
    private Transform cameraMainTransform;

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
    }
    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        //Player = controller.transform.gameObject;
        cameraMainTransform = Camera.main.transform;
        LockMouse();
    }

    private void FixedUpdate()
    {
        TeleportPlayer();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        if (groundedPlayer)
        {
            //tbr
            //playerSpeed = 10;
        }

        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x,0,movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //unbinds camera rotations based off movement
        if(movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Phase Enter"))
        {
            GameObject entrance = other.gameObject;
            GameObject exit = entrance.transform.parent.GetChild(0).gameObject;

            teleport = exit.transform.position;

            Debug.Log("crossed");
            crossed = true;
            //tbr
            //playerSpeed = playerSpeed * 5;
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

}

