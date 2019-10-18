using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private CubeController player;
    private AudioSource audioSource;
    [SerializeField] private float interactionRange = 0.5f; // What range the player needs to be inside of to interact
    [SerializeField] private GameObject interactionPrompt; // What displays when the player is in range - '!' or 'E', etc

    private void Awake()
    {
        player = FindObjectOfType<CubeController>();
        interactionPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Store distance to player
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceFromPlayer <= interactionRange)
        {
            interactionPrompt.SetActive(true); // Reveal prompt

            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
                interactionPrompt.SetActive(false); // Hide prompt
            }
        }
        else
        {
            if (interactionPrompt.activeSelf != false) // If the prompt is active...
            {
                interactionPrompt.SetActive(false); // Hide prompt
            }
        }
    }    

    private void Interact()
    {
        // Interaction code goes here, play audio, etc
    }
}
