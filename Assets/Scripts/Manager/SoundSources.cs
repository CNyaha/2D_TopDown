using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSources : MonoBehaviour
{
    private AudioSource _audioSource;

    public void Play(AudioClip clip, float soundEffectVolume, float soundEffectPitchVarance)
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }

        CancelInvoke();
        _audioSource.clip = clip;
        _audioSource.volume = soundEffectVolume;
        _audioSource.Play();
        _audioSource.pitch = 1f + Random.Range(-soundEffectPitchVarance, soundEffectPitchVarance);

        Invoke("Disable", clip.length + 2);
    }

    public void Disable()
    {
        _audioSource?.Stop();
        Destroy(this.gameObject);
    }

}
