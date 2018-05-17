using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Added
using UnityEngine.Networking;
using UnityEngine.Events;

public class PlayerAttack : NetworkBehaviour {

    ThirdPersonController player;

    void Start()
    {
        player = GetComponent<ThirdPersonController>();
    }

    public void BeginAttack()
    {
        if (this != null)
        {
            if (isServer)
            {
                RpcAttack();
            }
            else
            {
                CmdAttack();
            }
        }
    }

    public void EndAttack()
    {
        if (this != null)
        {
            if (isServer)
            {
                RpcDone();
            }
            else
            {
                CmdDone();
            }
        }
    }

    [Command]
    public void CmdAttack()
    {
        RpcAttack();
    }

    [ClientRpc]
    public void RpcAttack()
    {
        player.m_hitbox.enabled = true;
    }

    [Command]
    public void CmdDone()
    {
        RpcDone();
    }

    [ClientRpc]
    public void RpcDone()
    {
        player.m_hitbox.enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        ThirdPersonController enemy = other.gameObject.GetComponent<ThirdPersonController>();
        if (player.Attacking)
        {
            if (enemy != null && enemy != this)
            {
                if (isServer)
                {
                    enemy.RpcTakeDamage(10);
                }
                else
                {
                    enemy.CmdTakeDamage(10);
                }
            }
        }
    }
}
