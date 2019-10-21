using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogues")]
public class DialogueScriptableObject : ScriptableObject
{
    [TextArea(3, 10)]
    public string VoiceLine;

    public AudioClip VoiceAudio;
}
