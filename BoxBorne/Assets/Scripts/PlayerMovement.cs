using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            characterController.SimpleMove(new Vector3(1,0,0));
        }
    }
}
