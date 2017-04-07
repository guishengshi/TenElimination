using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
    public AudioClip buttonClip;
    public AudioClip destroyClip;
    public AudioSource musicSource;

    public void PlayButtonMusic() {
        musicSource.volume = 0.3f;
        musicSource.clip = buttonClip;
        musicSource.Play();
    }


    public void PlayDestroyMusic() {
        musicSource.volume = 0.1f;
        musicSource.clip = destroyClip;
        musicSource.Play();
    }
}
