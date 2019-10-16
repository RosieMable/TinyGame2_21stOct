using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhysics : MonoBehaviour
{
    [SerializeField] protected float m_moveSpeed;

    [SerializeField] protected float m_rotateSpeed;

    [SerializeField] protected LayerMask m_worldLayerMask;

    [SerializeField] protected Transform m_cameraTransform;

    [SerializeField] protected float m_positionOffsetScale;

    [SerializeField] protected float m_AwareRadius;

    [SerializeField] protected float m_AttackRadius;

    [SerializeField] protected DialogueScriptableObject dialogue;


    protected void Awake()
    {
        if(!m_cameraTransform)
            m_cameraTransform = Camera.main.transform;
    }

    protected void Update()
    {

        // Get movement direction based on camera orientation
        // Taken from https://gamedev.stackexchange.com/questions/89693/how-could-i-constrain-player-movement-to-the-surface-of-a-3d-object-using-unity
        //
        Vector3 movementDirection = Vector3.zero;
        
        //We need a move towards the player, there might be some issues with the rotation and direction of the character, 
       // since they changed based on the face they are
       //This is why I advise having possibly the enemies keeping a track of the main camera, which always keeps the correct directions
    }

   protected void UpdatePlayerRotation()
    {
        float rotation = Input.GetAxis("Mouse X") * m_rotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotation);
    }

   protected Vector3 GetInterpolatedHitNormal(RaycastHit hit)
    {
        // Get interpolated mesh normal at hit location 
        // (using hit.normal causes the player to snap in to position as it's not interpolated)
        // https://answers.unity.com/questions/50846/how-do-i-obtain-the-surface-normal-for-a-point-on.html
        //
        MeshCollider collider = (MeshCollider)hit.collider;
        Mesh mesh = collider.sharedMesh;
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;
        Vector3 n0 = normals[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 n1 = normals[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 n2 = normals[triangles[hit.triangleIndex * 3 + 2]];
        Vector3 baryCenter = hit.barycentricCoordinate;
        Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
        interpolatedNormal.Normalize();
        interpolatedNormal = hit.transform.TransformDirection(interpolatedNormal);
        return interpolatedNormal;
    }

    // Taken from https://gamedev.stackexchange.com/questions/89693/how-could-i-constrain-player-movement-to-the-surface-of-a-3d-object-using-unity
    //
    protected void UpdatePlayerTransform(Vector3 movementDirection)
    {
        RaycastHit hit;

        if (GetRaycastDownAtNewPosition(movementDirection, out hit))
        {
            Vector3 interpolatedNormal = GetInterpolatedHitNormal(hit);

            transform.rotation = Quaternion.FromToRotation(transform.up, interpolatedNormal) * transform.rotation; ;
            transform.position = hit.point + interpolatedNormal * m_positionOffsetScale;
        }
    }

    // Taken from https://gamedev.stackexchange.com/questions/89693/how-could-i-constrain-player-movement-to-the-surface-of-a-3d-object-using-unity
    //
    protected bool GetRaycastDownAtNewPosition(Vector3 movementDirection, out RaycastHit hitInfo)
    {
        Vector3 newPosition = transform.position;
        Ray ray = new Ray(transform.position + movementDirection * m_moveSpeed * Time.deltaTime, -transform.up);

        if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, m_worldLayerMask))
        {
            return true;
        }

        return false;
    }

    protected void onInteraction()
    {
        DialogueManager.Instance.DisplayLineAndPlayVoice(dialogue, dialogue.VoiceLine);
    }
}
