using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour {


    public enum PlayerType
   	{
        HUMAN, AI	
    };

    	public PlayerType player;
        public WarriorStates currentState = WarriorStates.IDLE;

    	protected Animator animator;
    	private Rigidbody myBody;

        /// Use this for initialization
        void Start ()
        {
            myBody = GetComponent<Rigidbody> ();
    		animator = GetComponent<Animator> ();
    	}

        public void UpdateHumanInput()
        {
            if (Input.GetAxis("Horizontal") > 0.1) {
                animator.SetBool("RUN", true);
            } else {
                animator.SetBool("RUN", false);
            }

            if (Input.GetAxis("Horizontal") < -0.1) {
                animator.SetBool("RUN", true);
            } else {
                animator.SetBool("RUN", false);
            }
   
            if (Input.GetAxis("Vertical") < -0.1) {
                animator.SetBool("WALK", true);
            } else {
                animator.SetBool("WALK", false);
            }

            if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) {
                animator.SetTrigger("JUMP");
            }

            if (Input.GetKeyDown (KeyCode.Z)) {
                animator.SetTrigger("PUNCH");
            }
        
            if (Input.GetKeyDown (KeyCode.X)) {
            	animator.SetTrigger("KICK");
            }

           	if (Input.GetKeyDown (KeyCode.C)) {
            	animator.SetTrigger("TAUNT");
            }
        
        }   

    /// Update is called once per frame
    void Update()
    {
        if (player == PlayerType.HUMAN) {
            UpdateHumanInput ();
        }
    }

    public Rigidbody body {
    		get {
    			return this.myBody;
    		}
    	}
    }

