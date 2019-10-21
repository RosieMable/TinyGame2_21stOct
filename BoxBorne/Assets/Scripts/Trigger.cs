using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> triggerObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CubeController>())
        {
            foreach (GameObject triggerObject in triggerObjects)
            {
                triggerObject.SetActive(!triggerObject.activeSelf);
            }
        }
    }
}
