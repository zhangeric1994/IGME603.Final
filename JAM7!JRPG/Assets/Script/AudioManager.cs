using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("BGM")]
    public AudioClip BGM;
    public AudioSource BGMSource;
    [Header("SoundFX")]
    public AudioClip[] SoundFX;
    public AudioSource[] EffectSource;
    [Header("Interface Sound")]
    public AudioClip[] InterfaceEffect;
    public AudioSource InterfaceSource;
    [Header("Pitch")]
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;
    [Header("Volume")]
    public float GlobalEffectVolume = 0.7f;
    public float GlobalBGMVoume = 0.45f;

    public static AudioManager Instance = null;

    private bool IsDelaying;
    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetGlobalEffectVolume(GlobalEffectVolume);
            SetGobalBGMVolume(GlobalBGMVoume);
            IsDelaying = false;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Set random pitch to the audiosource
    /// </summary>
    private void SetRandomPitch(AudioSource audioSource)
    {
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
        audioSource.pitch = randomPitch;
    }

    /// <summary>
    /// Play the background music
    /// </summary>
    public void PlayBGM()
    {
        BGMSource.clip = BGM;
        BGMSource.loop = true;
        BGMSource.Play();
    }

    public bool IsPlayingClip(string clipName)
    {
        foreach (AudioSource s in EffectSource)
        {
            if (s.isPlaying == true)
            {
                if (s.clip.name.Contains(clipName))
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Play a soundfx from loaded clips
    /// </summary>
    /// <param name="loop"> Whether loop the soundfx </param>
    public void PlaySoundEffect(string clipName, bool loop = false, bool pitch = true, float volume = 2, float DelayTime = 0)
    {
        AudioSource tempSource = null;
        foreach (AudioSource s in EffectSource)
        {
            if (s.isPlaying == false)
            {
                tempSource = s;
                if (volume != 2 && (volume <= 1 && volume >= 0))
                    tempSource.volume = volume;
                else
                    tempSource.volume = GlobalEffectVolume;
                break;
            }
        }
        if (tempSource == null)
        {
            Debug.Log("[AudioManager]: SoundFX AudioSource Runout ");
        }
        else
        {
            tempSource.loop = loop;

            if (pitch == true)
                SetRandomPitch(tempSource);
            else
                tempSource.pitch = 1;

            List<AudioClip> clips = new List<AudioClip>();
            foreach (AudioClip c in SoundFX)
                if (c)
                {
                    if (c.name.Contains(clipName))
                        clips.Add(c);
                }

            if (clips.Count == 0)
                Debug.Log("[AudioManager]: SoundFX Match Failure ");
            int index = Random.Range(0, clips.Count);
            tempSource.clip = clips[index];
            if(DelayTime != 0)
            {
                StartCoroutine(DelayPlay(tempSource,DelayTime));
            }
            else
                tempSource.Play();
        }
    }

    /// <summary>
    /// Play Interface Sound
    /// </summary>
    public void PlayInterfaceEffect(string clipName)
    {
        foreach (AudioClip c in InterfaceEffect)
        {
            if (c.name.Equals(clipName))
            {
                InterfaceSource.clip = c;
                InterfaceSource.Play();
                break;
            }
            else
                Debug.Log("[AudioManager]: InterfaceEffect Match Failure ");

        }
    }
    /// <summary>
    /// Stop a sound effect
    /// </summary>
    /// <param name="fadeOut"> Stop the soundFX by fadingout </param>
    /// <param name="fadeOutTime"> The fade out time, default 1f </param>
    public void StopSoundEffect(string clipName, bool fadeOut, float fadeoutTime = 1f)
    {
        foreach (AudioSource s in EffectSource)
        {
            if (s.isPlaying == true)
            {
                if (s.clip.name.Contains(clipName))
                {
                    if (fadeOut == true)
                        FadeOut(s, fadeoutTime);
                    else
                        s.Stop();
                }
            }
        }
    }

    public void StopBGM()
    {
        BGMSource.Stop();
    }

    public void SetGlobalEffectVolume(float volume)
    {
        GlobalEffectVolume = volume;
        foreach (AudioSource s in EffectSource)
        {
            s.volume = volume;
        }
    }

    public void SetGobalBGMVolume(float volume)
    {
        GlobalBGMVoume = volume;
        BGMSource.volume = volume;
    }

    private void FadeOut(AudioSource audioSource, float fadeOutTime)
    {
        StartCoroutine(AudioFadeOut.FadeOut(audioSource, fadeOutTime));
    }

    private IEnumerator DelayPlay(AudioSource audiosource, float time)
    {
        while(time > 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        audiosource.Play();
        yield return null;
    }
}




public static class AudioFadeOut
{
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

}

