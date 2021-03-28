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

    public void InAir()
    {
        playerAnimation.SetBool("inAir", true);
    }

    public void NotInAir()
    {
        playerAnimation.SetBool("inAir", false);
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

    public void IsJumping()
    {
        playerAnimation.SetBool("isJumping", true);
    }

    public void NotJumping()
    {
        playerAnimation.SetBool("isJumping", false);
    }

    public void Freefall()
    {
        playerAnimation.SetBool("isRunning" , false);
        playerAnimation.SetBool("inAir"     , true );
        playerAnimation.SetBool("isOnGround", false);
        playerAnimation.SetBool("isReaching", false);
        playerAnimation.SetBool("isJumping" , true );
        playerAnimation.Play("JumpLoop");
    }

    public void JumpStart()
    {
        playerAnimation.SetBool("isRunning" , false);
        playerAnimation.SetBool("inAir"     , false);
        playerAnimation.SetBool("isOnGround", true );
        playerAnimation.SetBool("isReaching", false);
        playerAnimation.SetBool("isJumping" , true );
        playerAnimation.Play("JumpStart");
    }

    public void Jumpsquat()
    {
        playerAnimation.SetBool("isRunning" , false);
        playerAnimation.SetBool("inAir"     , false);
        playerAnimation.SetBool("isOnGround", true );
        playerAnimation.SetBool("isReaching", false);
        playerAnimation.SetBool("isJumping" , false);
        playerAnimation.Play("JumpEnd");
    }

    public void Running()
    {
        playerAnimation.SetBool("isRunning" , true );
        playerAnimation.SetBool("inAir"     , false);
        playerAnimation.SetBool("isOnGround", true );
        playerAnimation.SetBool("isReaching", false);
        playerAnimation.SetBool("isJumping" , false);
        playerAnimation.Play("Running");
    }

    public void Idle()
    {
        playerAnimation.SetBool("isRunning" , false);
        playerAnimation.SetBool("inAir"     , false);
        playerAnimation.SetBool("isOnGround", true);
        playerAnimation.SetBool("isReaching", false);
        playerAnimation.SetBool("isJumping" , false);
        playerAnimation.Play("Idle");
    }

    public void Runstop()
    {

    }

    public void StopAnimations()
    {

    }
}
