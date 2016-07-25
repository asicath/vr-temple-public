using UnityEngine;
using System.Collections;

public class VoiceAction : ScriptAction {

    public static VoiceAction create(AudioClip clip, GameObject actor, float waitAfter = 1f, float waitBefore = 0)
    {
        return new VoiceAction { clip = clip, actor = actor, waitAfter = waitAfter, waitBefore = waitBefore };
    }

    public GameObject actor;
    public AudioClip clip;

    private AudioSource audioSource;

    protected override string getDebugId()
    {
        return "play " + clip.name + " from " + actor.name;
    }

    // Use this for initialization
    protected override void StartAction() {
        audioSource = actor.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }

    // Update is called once per frame
    protected override void UpdateAction() {
        if (!audioSource.isPlaying) complete();
    }

    public override void Instant()
    {
        if (audioSource != null) audioSource.Stop();
    }

}
