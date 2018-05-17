using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour {

    public float damage;

    public ThirdPersonController player;

    public void OnTriggerEnter(Collider other)
    {
        //ThirdPersonController enemy = other.gameObject.GetComponent<ThirdPersonController>();
        //if (player.Attacking)
        //{
        //    if (enemy != null && enemy != player)
        //    {
        //        enemy.TakeDamage(damage);
        //    }
        //}
    }
}
