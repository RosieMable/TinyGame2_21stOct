using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private PlayerStats player;
    [SerializeField] private int damage;

    private void Awake()
    {
        player = FindObjectOfType<PlayerStats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * 0.01f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            player.TakeDamage(damage);
        }   
    }
}
