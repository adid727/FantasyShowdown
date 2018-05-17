using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class ThirdPersonController : NetworkBehaviour
{

    [Header("Movement Variables")]
    [SerializeField]
    float m_inputDelay = 0.1f;
    [SerializeField]
    float m_forwardVel = 8.0f;
    [SerializeField]
    float m_jumpVel = 12.0f;
    [SerializeField]
    float m_downAccel = 0.75f;
    //Private Input Variables
    Vector3 m_velocity = Vector3.zero;
    Quaternion targetRotation;
    float m_forwardInput, m_vertInput;

    [Header("Player Variables")]
    [SerializeField]
    float m_health = 100;
    [SerializeField]
    public LayerMask m_ground;
    [SerializeField]
    public FighterStates currentState = FighterStates.IDLE;

    [Header("Camera Settings")]
    [SerializeField]
    float m_cameraDistance = 5.0f;
    [SerializeField]
    float m_cameraHeight = 2.5f;
    //Private Camera Variables
    Transform m_mainCamera;
    Vector3 m_cameraOffset;

    //Components
    Rigidbody m_rb;
    Animator m_anim;
    NetworkAnimator m_Nanim;
    public BoxCollider m_hitbox;

    AudioSource audioPlayer;

    public GameObject m_model;


    //For testing locally
    public bool test = false;

    void Start()
    {
        if(!isLocalPlayer)
        {
            this.enabled = false;
            return;
        }

        //Reference to Rigidbody and Animator attached to this script
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_Nanim = GetComponent<NetworkAnimator>();
        m_hitbox = GetComponent<BoxCollider>();
        m_hitbox.enabled = false;
        audioPlayer = GetComponent<AudioSource>();

        m_cameraOffset = new Vector3(0.0f, m_cameraHeight, -m_cameraDistance);
        m_mainCamera = Camera.main.transform;
        MoveCamera();
    }

    void GetInput()
    {
        m_forwardInput = Input.GetAxis("Horizontal");
        m_vertInput = Input.GetAxis("Vertical");
    }

    void GetAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_anim.SetBool("Punch", true);
            m_anim.SetBool("Run", false);

        }
        else
        {
            m_anim.SetBool("Punch", false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            m_anim.SetBool("Kick", true);
            m_anim.SetBool("Run", false);
        }
        else
        {
            m_anim.SetBool("Kick", false);
        }

        if (Input.GetKey(KeyCode.C))
        {
            m_anim.SetBool("Defend", true);
        }
        else
        {
            m_anim.SetBool("Defend", false);
        }
    }

    void Update()
    {
        //Check to see if still alive
        if (currentState == FighterStates.DEAD || test == true)
        {
            return;
        }

        GetInput();
        GetAttackInput();

        if (m_health <= 0 && currentState != FighterStates.DEAD)
        {
            m_Nanim.SetTrigger("Dead");
        }
    }

    public void playSound(AudioClip sound)
    {
        GameUtils.playSound(sound, audioPlayer);
    }

    void FixedUpdate()
    {
        //Check to see if still alive or blocking
        if (currentState == FighterStates.DEAD || currentState == FighterStates.DEFEND || currentState == FighterStates.TAKE_HIT || test == true)
        {
            return;
        }

        Run();
        Jump();
        m_rb.velocity = transform.TransformDirection(m_velocity);

        //Update the camera
        MoveCamera();
    }

    void Run()
    {
        if (Mathf.Abs(m_forwardInput) > m_inputDelay)
        {
            //Move
            m_velocity.z = m_forwardVel * m_forwardInput;
            m_anim.SetBool("Run", true);

            //Flip the model in the correct direction
            if (m_forwardInput > 0.1f)
            {
                targetRotation = Quaternion.AngleAxis(90, Vector3.up);
                transform.rotation = targetRotation;

            }
            else if (m_forwardInput < -0.1f)
            {
                targetRotation = Quaternion.AngleAxis(-90, Vector3.up);
                transform.rotation = targetRotation;
                //Invert velocity
                m_velocity.x *= -1;
                m_velocity.z *= -1;
            }
        }
        else
        {
            //Zero velocity
            m_velocity.z = 0;
            m_anim.SetBool("Run", false);
        }
    }

    void Jump()
    {
        if ((Input.GetKey(KeyCode.Space) || m_vertInput > 0.0f) && isGrounded())
        {
            //Jump
            m_velocity.y = m_jumpVel;
            m_anim.SetTrigger("Jump");
        }
        else if (isGrounded())
        {
            //If on the ground
            m_velocity.y = 0;
        }
        else
        {
            //If falling
            m_velocity.y -= m_downAccel;
        }
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f, m_ground);
    }

    [Command]
    public void CmdTakeDamage(float damage)
    {
        RpcTakeDamage(damage);
    }

    [ClientRpc]
    public void RpcTakeDamage(float damage)
    {
        if (!Invulnerable)
        {
            if (Defending)
            {
                damage *= 0.2f;
            }
            if (m_health >= damage)
            {
                m_health -= damage;
            }
            else
            {
                m_health = 0;
            }

            if (m_health > 0)
            {
                m_Nanim.SetTrigger("Hit");
            }
        }
    }

    //Getters to check current state
    public bool Attacking
    {
        get
        {
            return currentState == FighterStates.ATTACK;
        }
    }

    public bool Invulnerable
    {
        get
        {
            return currentState == FighterStates.TAKE_HIT
                || currentState == FighterStates.TAKE_HIT_DEFEND
                    || currentState == FighterStates.DEAD;
        }
    }

    public bool Defending
    {
        get
        {
            return currentState == FighterStates.DEFEND
                || currentState == FighterStates.TAKE_HIT_DEFEND;
        }
    }

    void MoveCamera()
    {
        m_mainCamera.position = transform.position;
        m_mainCamera.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        m_mainCamera.Translate(m_cameraOffset);
        m_mainCamera.LookAt(transform);
    }

}
