using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    CharacterController controller;
    Transform cameraMainTransform;
    public InputActionReference movementControl;
    [SerializeField] public float rotationSpeed = 20f;
    private PlayerController player;
    [SerializeField] private Vector3 playerVelocity;
    public bool onground;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
    }

    //state events
    public void OnGroundState()
    {
        //Run();
    }


    //methods to run in state events
    public void Run()
    {
        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0;

        // unbinds camera rotations based off movement
        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.unscaledDeltaTime * rotationSpeed);
        }
        onground = true;
    }

    private void OnEnable()
    {
        movementControl.action.Enable();
        //actionOne.action.Enable();
        //actionTwo.action.Enable();
        //actionThree.action.Enable();
        //jumpControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        //actionOne.action.Disable();
        //actionTwo.action.Disable();
        //actionThree.action.Disable();
        //jumpControl.action.Disable();
    }

}
