using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private CubeController player;
    private AudioSource audioSource;
    [SerializeField] private float interactionRange = 0.5f; // What range the player needs to be inside of to interact
    [SerializeField] private GameObject interactionPrompt; // What displays when the player is in range - '!' or 'E', etc
    [SerializeField] DialogueScriptableObject NPCLine;
    private UIManager uI;

    bool isInteracting;

    private void Awake()
    {
        player = FindObjectOfType<CubeController>();
        interactionPrompt.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        isInteracting = false;
        uI = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Store distance to player
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceFromPlayer <= interactionRange && isInteracting == false)
        {
            interactionPrompt.SetActive(true); // Reveal prompt
            uI.OnInteraction();


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
            uI.interact.SetActive(false);
        }
    }    

    private void Interact()
    {
        isInteracting = true;
        uI.interact.SetActive(false);
        // Interaction code goes here, play audio, etc
        DialogueManager.Instance.DisplayLineAndPlayVoice(NPCLine, NPCLine.VoiceAudio, audioSource);
    }
}
