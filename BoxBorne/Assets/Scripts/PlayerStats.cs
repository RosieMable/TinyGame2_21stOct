using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    private CubeController player;
    [SerializeField] private int health;
    private AudioSource audioSource;
    [SerializeField] AudioClip swingClip;
    [SerializeField] private GameObject damageArea;
    private float attackDelay = 0;
    [SerializeField] private float attackCooldown = 0.3f;

    private void Awake()
    {
        player = FindObjectOfType<CubeController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Time.time > attackDelay)
            {
                attackDelay = Time.time + attackCooldown;
                Attack();
            }
        }   
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
        // GameOver overlay/scene transition

        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
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
        StartCoroutine(PlayAttackAnimation(0.2f));
    }

    private IEnumerator PlayAttackAnimation(float delay)
    {
        audioSource.clip = swingClip;
        audioSource.Play();
        damageArea.SetActive(true);
        Transform childTransform = transform.Find("Chad Boxington");
        transform.Find("Chad Boxington").Rotate(new Vector3(0,-45,0));

        yield return new WaitForSeconds(delay);

        transform.Find("Chad Boxington").Rotate(new Vector3(0, 45, 0));
        damageArea.SetActive(false);
    }
}
