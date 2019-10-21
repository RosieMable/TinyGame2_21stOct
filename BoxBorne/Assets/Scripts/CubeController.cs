using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] public float m_moveSpeed;

    [SerializeField] private float m_rotateSpeed;

    [SerializeField] private LayerMask m_worldLayerMask;

    [SerializeField] private Transform m_cameraTransform;

    [SerializeField] private float m_positionOffsetScale;

    private Rigidbody ChadRB;


    private bool isTeleporting;

    public bool isKnockedBack;

    public void TeleportPlayer(Transform target, float duration)
    {
        if (isTeleporting) { return; }
        StartCoroutine(TeleportPlayer_Coroutine(target, duration));
    }

    private IEnumerator TeleportPlayer_Coroutine(Transform target, float duration)
    {
        ChadRB = GetComponent<Rigidbody>();

        ChadRB.isKinematic = true;
        

        isTeleporting = true;

        float remainingTime = duration;
        Vector3 startLocation = transform.position;

        while (remainingTime > 0)
        {
            float d = 1.0f - (remainingTime / duration);
            transform.position = Vector3.Lerp(startLocation, target.position, d);
            yield return null;
            remainingTime -= Time.deltaTime;
        }

        transform.position = target.position;
        isTeleporting = false;
        ChadRB.isKinematic = false;

    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        if (isTeleporting)
        {
            return;
        }

        // Get movement direction based on camera orientation
        // Taken from https://gamedev.stackexchange.com/questions/89693/how-could-i-constrain-player-movement-to-the-surface-of-a-3d-object-using-unity
        //
        Vector3 movementDirection = Vector3.zero;

        if (!isKnockedBack)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                movementDirection += m_cameraTransform.up;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                movementDirection += -m_cameraTransform.up;
            }

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                movementDirection += m_cameraTransform.right;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                movementDirection += -m_cameraTransform.right;
            }
        }
        
        movementDirection.Normalize();

        // Move player
        UpdatePlayerTransform(movementDirection);

        // Rotate player
        UpdatePlayerRotation();        
    }

    private void UpdatePlayerRotation()
    {
        float rotation = Input.GetAxis("Mouse X") * m_rotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotation);
    }

    private Vector3 GetInterpolatedHitNormal(RaycastHit hit)
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
    private void UpdatePlayerTransform(Vector3 movementDirection)
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
    private bool GetRaycastDownAtNewPosition(Vector3 movementDirection, out RaycastHit hitInfo)
    {
        Vector3 newPosition = transform.position;
        Ray ray = new Ray(transform.position + movementDirection * m_moveSpeed * Time.deltaTime, -transform.up);

        if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, m_worldLayerMask))
        {
            return true;
        }

        return false;
    }
}
