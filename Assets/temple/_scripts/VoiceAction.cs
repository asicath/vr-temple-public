using UnityEngine;
using System.Collections;

public class VoiceAction : ScriptAction {

    public GameObject actor;
    public AudioClip clip;

    private AudioSource audioSource;

	// Use this for initialization
	public override void Start () {
        audioSource = actor.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }
	
	// Update is called once per frame
	public override void Update () {
        if (!audioSource.isPlaying) onComplete();
    }

    public override void Instant()
    {
        audioSource.Stop();
    }

}
