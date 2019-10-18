using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private CubeController player;
    [SerializeField] private int health;

    private void Awake()
    {
        player = FindObjectOfType<CubeController>();
    }

    public void TakeDamage(int damageValue)
    {
        health -= damageValue;
        CheckHealth();

        StartCoroutine(KnockbackEffect(0.5f));
    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        // Needs to be expanded on.
        print("I died");
    }
    private IEnumerator KnockbackEffect(float movementLockoutDuration)
    {
        player.isKnockedBack = true;
        transform.position = Vector3.Lerp(transform.position, transform.position - transform.forward * 0.5f, movementLockoutDuration * player.m_moveSpeed);
        yield return new WaitForSeconds(movementLockoutDuration);
        player.isKnockedBack = false;
    }
}
