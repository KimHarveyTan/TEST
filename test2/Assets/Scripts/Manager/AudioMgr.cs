using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    Damage,
    Dead,
}

public class AudioMgr : Singleton<AudioMgr>
{
    [SerializeField] List<AudioClip> clipList = new List<AudioClip>();
    [SerializeField] AudioSource source;

    public void PlaySFX()
    {
        source.clip = clipList[0];
        source.Play();
    }
}
