using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanPC : GameController {

    void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        
        //Reference to Rigidbody and Animator attached to this script
        //m_rb = GetComponent<Rigidbody>();
        //m_anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //UpdateMovement();
    }

    void Update()
    {
        //CheckInput();
    }

    void OnCollisionEnter(Collision col)
    {
        //CheckGrounded(col);
    }

}
