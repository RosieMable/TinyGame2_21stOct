using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : EnemyPhysics
{
    // Detection / Combat
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float detectionRange = 0.5f;
    protected float originalDetectionRange;
    [SerializeField] protected float minimumRange = 0.2f;
    protected PlayerStats player;
    private bool detected = false;

    // Stats
    [SerializeField] private int health;

    // Audio
    private AudioSource audioSource;
    [SerializeField] private AudioClip detectedAudioClip;
    [SerializeField] private AudioClip[] damageClips;

    // Patrol
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();
    private int patrolPointIndex = 0;

    // Misc
    protected bool knockedBack = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        originalDetectionRange = detectionRange;
        player = FindObjectOfType<PlayerStats>();
    }

    private void Update()
    {        
        UpdateCharacterTransform(transform.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (!knockedBack)
        {
            if (distanceToPlayer <= detectionRange)
            {
                transform.LookAt(player.transform.position, transform.up);

                if (!detected)
                {
                    audioSource.clip = detectedAudioClip;
                    audioSource.Play();
                    detected = true;
                }                

                if (detectionRange != originalDetectionRange * 2)
                {
                    detectionRange = originalDetectionRange * 2;
                }                

                if (distanceToPlayer > minimumRange)
                {
                    MoveToLocation(player.transform);
                }
            }
            else
            {
                detected = false;
                detectionRange = originalDetectionRange;
                Patrol();
            }
        }        
    }

    protected void MoveToLocation(Transform targetTransform)
    {
        transform.LookAt(targetTransform, transform.up);
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, m_moveSpeed * Time.deltaTime);        
    }

    protected void Patrol()
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

    public void TakeDamage(Transform damageSource, int damageValue)
    {
        health -= damageValue;
        CheckHealth();

        int audioClipToPlay = Random.Range(0, damageClips.Length);
        if (!audioSource.isPlaying)
        {
            audioSource.clip = damageClips[audioClipToPlay];
            audioSource.Play();
        }

        StartCoroutine(KnockbackEffect(damageSource, 0.5f));
    }

    protected void CheckHealth()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject); // Update later for a death animation/sound effect to be played before destruction
    }

    protected IEnumerator KnockbackEffect(Transform sourcePoint, float movementLockoutDuration)
    {
        knockedBack = true;
        transform.position = Vector3.Lerp(transform.position, transform.position - transform.forward * 0.5f, 10 * Time.deltaTime);
        yield return new WaitForSeconds(movementLockoutDuration);
        knockedBack = false;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
