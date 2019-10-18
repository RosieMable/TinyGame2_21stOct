using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public static DialogueManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (dialogueText == null)
            GameObject.FindGameObjectWithTag("Dialogue");
    }

    private void Update()
    {
        if (dialogueText == null)
            GameObject.FindGameObjectWithTag("Dialogue");
    }


    public void DisplayLineAndPlayVoice(DialogueScriptableObject dialogue, AudioClip VoiceAudio, AudioSource source)
    {
        StartCoroutine(DisplayTextWhileSound(dialogue, VoiceAudio, source));
    }


    IEnumerator DisplayTextWhileSound(DialogueScriptableObject Cdialogue, AudioClip CvoiceLine, AudioSource source)
    {
        if (dialogueText == null)
            GameObject.FindGameObjectWithTag("Dialogue");

        dialogueText.text = Cdialogue.VoiceLine;

        source.PlayOneShot(CvoiceLine);

        yield return new WaitForSeconds(CvoiceLine.length);

        source.clip = null;

        dialogueText.text = string.Empty;

    }

}
