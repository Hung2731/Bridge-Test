using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private List<AudioClip> m_BgmClips;
    [SerializeField] private List<AudioClip> m_SfxClips;
    [SerializeField] private AudioSource m_BgmSource;
    [SerializeField] private AudioSource m_SfxSource;
    public void PlayBGM(BackgroundMusic bgmType)
    {
        m_BgmSource.clip = m_BgmClips[(int)bgmType];
        m_BgmSource.Play();
    }

    public void PlaySFX(SoundEffect sfxType)
    {
        m_SfxSource.PlayOneShot(m_SfxClips[(int)sfxType]);
    }
}

public enum BackgroundMusic
{
    MainTheme,
    GamePlay,
}

public enum SoundEffect
{
    ButtonClick,
}
