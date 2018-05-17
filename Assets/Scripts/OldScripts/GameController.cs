using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Added
using UnityEngine.Networking;
using UnityEngine.Events;

public class GameController : NetworkBehaviour {

    [Header("Movement Variables")]
    [SerializeField]
    float m_movementSpeed = 7.0f;
    [SerializeField]
    float m_jumpHeight = 300.0f;
    [SerializeField]
    bool m_isGrounded = true;
    [SerializeField]
    bool m_isStunned = false;

    public Rigidbody m_rb;
    public Animator m_anim;
    public NetworkAnimator m_Nanim;
    public GameObject m_model;
    public SphereCollider jabCollider;

    void Start () {

        if (!isLocalPlayer)
        {
            //If this is not the local player, then destroy the script and return
            //Destroy(this);
            //return;
        }

        //Reference to Rigidbody and Animator attached to this script
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_Nanim = GetComponent<NetworkAnimator>();
    }

	void FixedUpdate ()
    {
        //Setting up horizontal movement
        float moveAmount = Input.GetAxis("Horizontal");

        //Move the object on the horizontal access and multiply it by the set speed and delta time
        Vector3 deltaTranslation = new Vector3(moveAmount, 0, 0) * m_movementSpeed * Time.deltaTime;
        m_rb.MovePosition(transform.position + deltaTranslation);

        //Set the animations for Idle and Run depending on the Horizontal Axis value
        //If the Horizontal Axis is positive then flip the sprite to the right
        if (Input.GetAxis("Horizontal") > 0.1)
        {
            m_model.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
            m_anim.SetBool("Run", true);
            m_anim.SetBool("Idle", false);
        }
        //If the Horizontal Axis is negative then flip the sprite to the left
        else if (Input.GetAxis("Horizontal") < -0.1)
        {
            m_model.transform.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
            m_anim.SetBool("Run", true);
            m_anim.SetBool("Idle", false);
        }
        //Idle
        else
        {
            m_anim.SetBool("Run", false);
            m_anim.SetBool("Idle", true);
        }

        //Jump!
        if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded)
        {
            m_rb.AddForce(0, m_jumpHeight, 0);
            m_anim.SetTrigger("Jump");
            m_isGrounded = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_Nanim.SetTrigger("Jab");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            m_Nanim.SetTrigger("Hikick");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            m_Nanim.SetTrigger("Spinkick");
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            m_isGrounded = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Hit!");
        }
    }

    //Temp
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width - 260, 10, 250, 130), "Interaction");
        GUI.Label(new Rect(Screen.width - 250, 30, 250, 30), "Arrow Keys to Move");
        GUI.Label(new Rect(Screen.width - 250, 50, 250, 30), "Space to Jump");
        GUI.Label(new Rect(Screen.width - 250, 70, 250, 30), "Z for Jabs (Hit in succession for combo)");
        GUI.Label(new Rect(Screen.width - 250, 90, 250, 30), "X for Kicks (Hit in succession for combo)");
        GUI.Label(new Rect(Screen.width - 250, 110, 250, 30), "C for Drill Kick");
        if (GUI.Button(new Rect(Screen.width - 260, 145, 250, 20), "Quit"))
        {
            Application.Quit();
        }
    }

    //Temp Detection
    //Move to its own script after ALPHA
    void AttackHitbox(string attackType)
    {
        switch (attackType)
        {
            case "JAB":
                jabCollider.enabled = !jabCollider.enabled;
                break;

            default:
                break;
        }
    }
}
