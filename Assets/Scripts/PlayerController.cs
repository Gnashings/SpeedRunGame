using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference jumpControl;
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject poggers;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 10.0f;
    [SerializeField] private float jumpHeight = 10.0f;
    [SerializeField] private float gravityValue = -9.81f;
    private Transform cameraMainTransform;
    [SerializeField] private float rotationSpeed = 4f;

    public bool crossed = false;
    public GameObject player;
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

        if (crossed == true)
        {
            player.transform.localPosition = poggers.transform.position;
            crossed = false;
        }
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
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


        //Debug.Log(player.transform.localPosition);
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
            /***
            GameObject entrance = other.gameObject;
            //Debug.Log(entrance);
            GameObject exit = entrance.transform.parent.GetChild(0).gameObject;
            //Debug.Log(exit);
            Debug.Log("player posit1" + Player.transform.position);
            Debug.Log("Existe posit1" + exit.transform.position);
            Player.transform.position = exit.transform.position;
            Debug.Log("player posit2" + Player.transform.position);
            Debug.Log("Existe posit2" + exit.transform.position);
            ***/

            //controller.gameObject.transform.position = poggers.transform.position;

            //GameObject exit = other.gameObject.transform.parent.transform.GetChild(0);
            Debug.Log("crossed");
            crossed = true;

        }
    }

    private void TeleportPlayer()
    {

    }

}

