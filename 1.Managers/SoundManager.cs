using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : TSingleton<SoundManager>
{
    [SerializeField] AudioSource _bgPlayer;
    [SerializeField] AudioSource _sfxPlayer;
    [SerializeField] AudioSource[] _fxPlayer;

    float _bgvolume;
    float _fxvolume;
    bool _bgmMute;
    bool _fxMute;
    private void Awake()
    {
        Init();
        
    }
    protected override void Init()
    {
        base.Init();
    }

    public void BGSoundPlay(AudioClip clip)
    {
        _bgPlayer.clip = clip;
        _bgPlayer.loop = true;
        _bgPlayer.Play();
    }
    public void SfxSoundPlay(AudioClip clip)
    {
        _sfxPlayer.PlayOneShot(clip);
    }
}
