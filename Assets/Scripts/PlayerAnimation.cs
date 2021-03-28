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

    public void NotIsOnGround()
    {
        playerAnimation.SetBool("isOnGround", false);
    }

    public void IsReaching()
    {
        playerAnimation.SetBool("isReaching", true);
    }

    public void NotIsReaching()
    {
        playerAnimation.SetBool("isReaching", false);
    }

    public void StopAnimations()
    {

    }
}
