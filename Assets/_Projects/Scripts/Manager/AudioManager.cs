using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public float value;

    public Slider musicSlider;
    public Slider soundFXSlider;

    public Sound[] sounds;

    [Range(0f, 1f)]
    float m_MusicVolume;
    public float musicVolume { get { return m_MusicVolume; } set { m_MusicVolume = value; OnMusicVolumeSave(); } }

    private void OnMusicVolumeSave()
    {
        Sound[] musicSounds = Array.FindAll(sounds, sound => sound.type == Sound.Type.MUSIC);
        foreach (Sound item in musicSounds)
        {
            item.source.volume = m_MusicVolume;
        }
    }

    [Range(0f, 1f)]
    float m_SoundFXVolume;
    public float soundVolume { get { return m_SoundFXVolume; } set { m_SoundFXVolume = value; OnSoundFXVolumeSave(); } }

    private void OnSoundFXVolumeSave()
    {
        Sound[] musicSounds = Array.FindAll(sounds, sound => sound.type == Sound.Type.EFFECT);
        foreach (Sound item in musicSounds)
        {
            item.source.volume = m_MusicVolume;
        }
    }
    #region Singleton

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.isLoop;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

   
    public void PlaySFX(string name, float volumeScale)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.PlayOneShot(s.clip, volumeScale);
        }
    }

    public void PlayLoopSFX(string name, float volumeScale)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
            s.source.loop = true;
        }
    }
    public void StopSFX(string name, float volumeScale)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source.isPlaying) s.source.Stop();
    }

    public void ChangeMainTheme(string name)
    {
        Sound[] s = Array.FindAll<Sound>(sounds, sound => sound.type == Sound.Type.MUSIC);
        foreach (Sound sound in s)
        {
            if (sound.name != name) sound.source.Stop();
        }
        PlaySFX(name, 1f);
    }

    public void OnMusicVolumeChange()
    {
        if (musicSlider != null)
        {
            Sound[] musicSounds = Array.FindAll(sounds, sound => sound.type == Sound.Type.MUSIC);
            foreach (Sound item in musicSounds)
            {
                item.source.volume = musicSlider.value;
            }
        }
    }

    public void OnSoundFXVolumeChange()
    {
        if (soundFXSlider != null)
        {
            Sound[] musicSounds = Array.FindAll(sounds, sound => sound.type == Sound.Type.EFFECT);
            foreach (Sound item in musicSounds)
            {
                item.source.volume = soundFXSlider.value;
            }
        }
    }

    public void MuteAudio()
    {
        Sound[] musicSounds = Array.FindAll(sounds, sound => sound.type == Sound.Type.EFFECT);
        foreach (Sound sound in musicSounds)
        {
            sound.source.volume = 0;
        }
    }
    public void StopAllAudio()
    {
        Sound[] musicSounds = Array.FindAll(sounds, sound => sound.type == Sound.Type.EFFECT);
        foreach (Sound sound in musicSounds)
        {
            if (sound.source.isPlaying)
            {
                sound.source.Stop();
            }
        }
    }
    public void NonMuteAudio()
    {
        Sound[] musicSounds = Array.FindAll(sounds, sound => sound.type == Sound.Type.EFFECT);
        foreach (Sound sound in musicSounds)
        {
            sound.source.volume = 1;
        }
    }


}
