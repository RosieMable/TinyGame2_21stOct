using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class DialogueManager : MonoBehaviour
{
    public Text dialogueText;

    public AudioSource source;

    private static readonly object padlock = new object();
    private static DialogueManager instance = null;
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DialogueManager();
                    }
                }
            }
            return instance;
        }
    }
 

    public void DisplayLineAndPlayVoice(DialogueScriptableObject dialogue, AudioClip VoiceAudio)
    {
        StartCoroutine(DisplayTextWhileSound(dialogue, VoiceAudio));
    }


    IEnumerator DisplayTextWhileSound(DialogueScriptableObject Cdialogue, AudioClip CvoiceLine)
    {
        dialogueText.text = Cdialogue.VoiceLines[0];

        source.PlayOneShot(CvoiceLine);

        yield return new WaitForSeconds(CvoiceLine.length);

        source.clip = null;

        dialogueText.text = string.Empty;

    }

}
