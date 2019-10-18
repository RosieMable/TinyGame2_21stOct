using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            player.TakeDamage(transform, damage);

            StartCoroutine(KnockbackEffect(transform, 0.5f));
        }
    }
}
