using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform m_target;
    [SerializeField] private float m_speed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent.GetComponent<CubeController>().TeleportPlayer(m_target, m_speed);
        }
    }
}
