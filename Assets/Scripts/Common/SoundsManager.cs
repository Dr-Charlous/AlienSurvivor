using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance;
    [SerializeField] AudioSource[] _source;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("2 Sound Instances");
    }

    public void ChangeSounds(AudioClip clip, int value)
    {
        _source[value].clip = clip;
    }

    public void PlaySound(int value)
    {
        _source[value].Play();
    }

    public void PlaySound(AudioClip clip, int value)
    {
        ChangeSounds(clip, value);
        PlaySound(value);
    }
}
