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

    public void TakeDamage(Transform damageSource, int damageValue)
    {
        health -= damageValue;
        CheckHealth();

        StartCoroutine(KnockbackEffect(damageSource, 0.5f));
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
    private IEnumerator KnockbackEffect(Transform sourcePoint, float movementLockoutDuration)
    {
        player.isKnockedBack = true;
        transform.position = Vector3.Lerp(transform.position, transform.position + sourcePoint.forward * 0.5f, 10 * Time.deltaTime);
        yield return new WaitForSeconds(movementLockoutDuration);
        player.isKnockedBack = false;
    }

    private void Attack()
    {
        // Play Attack Animation
        // Animation has attached collider, damage class may need to be on the weapon?
    }
}
