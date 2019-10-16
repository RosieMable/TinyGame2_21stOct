﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyPhysics
{
    // Type
    private enum EnemyType { Melee, Ranged }
    [SerializeField] private EnemyType enemyType;

    // Detection / Combat
    [SerializeField] protected float detectionRange = 0.5f;
    private float originalDetectionRange;
    [SerializeField] protected float attackRange = 0.2f;
    [SerializeField] protected float minimumRange = 0.2f;
    private float attackDelay;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private GameObject rangedProjectile = null;
    private CubeController player;

    // Stats
    [SerializeField] private int health;

    // Audio
    private AudioSource audioSource;
    [SerializeField] private AudioClip detectedAudioClip;

    // Patrol
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();
    private int patrolPointIndex = 0;

    // Misc
    private bool knockedBack = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        originalDetectionRange = detectionRange;
        player = FindObjectOfType<CubeController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(player.transform.position, 5);
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

    private void MoveToLocation(Transform targetTransform)
    {
        transform.LookAt(targetTransform, transform.up);
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, m_moveSpeed * Time.deltaTime);        
    }

    private void Patrol()
    {
        if (patrolPoints.Count > 0)
        {
            if (Vector3.Distance(transform.position, patrolPoints[patrolPointIndex].position) < 0.1f)
            {
                if (patrolPointIndex + 1 < patrolPoints.Count)
                {
                    patrolPointIndex++;
                }
                else
                {
                    patrolPointIndex = 0;
                }
            }

            MoveToLocation(patrolPoints[patrolPointIndex]);
        }
    }

    private void Attack()
    {
        if (enemyType == EnemyType.Melee)
        {
            // Melee Attack Code
        }
        else
        {
            // Ranged Attack Code
        }
    }

    public void TakeDamage(Vector3 sourcePoint, int damageValue)
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
        Destroy(gameObject); // Update later for a death animation/sound effect to be played before destruction
    }

    private IEnumerator KnockbackEffect(float movementLockoutDuration)
    {
        knockedBack = true;
        transform.position = Vector3.Lerp(transform.position, transform.position - transform.forward * 0.5f, movementLockoutDuration * m_moveSpeed);
        yield return new WaitForSeconds(movementLockoutDuration);
        knockedBack = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}