using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator playerAnimation;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimation = GetComponent<Animator>();
        playerAnimation = GameObject.Find("runningAnim").GetComponent<Animator>();
    }

    public void IsRunning()
    {
        playerAnimation.SetBool("isRunning", true);
    }

    public void NotRunning()
    {
        playerAnimation.SetBool("isRunning", false);
    }

    public void IsOnGround()
    {
        playerAnimation.SetBool("isOnGround", true);
    }

    public void NotOnGround()
    {
        playerAnimation.SetBool("isOnGround", false);
    }

    public void IsReaching()
    {
        playerAnimation.SetBool("isReaching", true);
    }

    public void NotReaching()
    {
        playerAnimation.SetBool("isReaching", false);
    }

    public void Freefall()
    {
        playerAnimation.SetBool("isOnGround", false);
        playerAnimation.SetBool("isReaching", false);
    }

    public void JumpStart()
    {
        playerAnimation.SetBool("isReaching", false);
    }

    public void JumpEnd()
    {
        playerAnimation.SetBool("isOnGround", true );
        playerAnimation.SetBool("isReaching", false);
    }

    public void JumpSquatIdle()
    {
        playerAnimation.SetBool("isRunning" , false);
        playerAnimation.SetBool("isOnGround", true);
        playerAnimation.SetBool("isReaching", false);
    }
}
