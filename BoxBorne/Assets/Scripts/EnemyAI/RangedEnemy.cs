using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private float attackRange = 0.5f;
    private float attackDelay;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private GameObject rangedProjectile = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(5);
        }

        UpdateCharacterTransform(transform.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (!knockedBack)
        {
            if (distanceToPlayer <= detectionRange)
            {
                transform.LookAt(player.transform.position, transform.up);

                if (detectionRange != originalDetectionRange * 2)
                {
                    detectionRange = originalDetectionRange * 2;
                }

                // Play detectedAudioClip here

                if (distanceToPlayer > minimumRange)
                {
                    MoveToLocation(player.transform);
                }

                if (distanceToPlayer <= attackRange)
                {
                    if (Time.time > attackDelay)
                    {
                        attackDelay = attackDelay + attackCooldown;
                        Attack();
                    }
                }
            }
            else
            {
                detectionRange = originalDetectionRange;
                Patrol();
            }
        }
    }

    private void Attack()
    {
        GameObject _Projectile = Instantiate(rangedProjectile, transform.position + transform.forward, Quaternion.identity, null);
    }
}
