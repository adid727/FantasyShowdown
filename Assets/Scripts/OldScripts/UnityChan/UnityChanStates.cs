using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class UnityChanStates : NetworkBehaviour
{

    public enum UnityStates
    {
        Idle, RUN, JAB, UPPERCUT, HIKICK, SPINKICK, FLIPKICK, SCREWKICK, JUMP, LAND, DEATH, RISE
    }

    public enum PlayerType
    {
        HUMAN, AI
    };

    public PlayerType player;
    public UnityStates currentState = UnityStates.Idle;

    protected Animator animator;
    private Rigidbody rb;

    /// Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void UpdateHumanInput()
    {
        if (Input.GetAxis("Horizontal") > 0.1)
        {
            transform.localScale = new Vector3(1, 1, 1);
            ///animator.SetBool("Run", true);
        }
        else if (Input.GetAxis("Horizontal") < -0.1)
        {
            transform.localScale = new Vector3(1, 1, -1);
            //animator.SetBool("Run", true);
        }
        else
        {
            //animator.SetBool("Run", false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            animator.SetTrigger("Jump");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Jab");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("Hikick");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetTrigger("Spinkick");
        }

    }

    /// Update is called once per frame
    void Update()
    {
        if (player == PlayerType.HUMAN)
        {
            UpdateHumanInput();
        }
    }
}
