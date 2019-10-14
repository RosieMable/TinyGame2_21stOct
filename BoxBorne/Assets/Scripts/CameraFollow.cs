using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_target;
    [SerializeField] private float m_distance;
    [SerializeField] private float m_height;

    [SerializeField] private float m_damping;
    [SerializeField] private float m_rotationDamping;

    // Taken from https://gamedev.stackexchange.com/questions/89693/how-could-i-constrain-player-movement-to-the-surface-of-a-3d-object-using-unity
    //
    private void FixedUpdate()
    {
        // Calculate and set camera position
        Vector3 desiredPosition = this.m_target.TransformPoint(0, this.m_height, -this.m_distance);
        this.transform.position = Vector3.Lerp(this.transform.position, desiredPosition, Time.deltaTime * this.m_damping);

        // Calculate and set camera rotation
        Quaternion desiredRotation = Quaternion.LookRotation(this.m_target.position - this.transform.position, this.m_target.up);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, desiredRotation, Time.deltaTime * this.m_rotationDamping);
    }
}
