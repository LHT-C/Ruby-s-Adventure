using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 播放音乐 音效
/// </summary>
public class AudioManager : MonoBehaviour
{
    //单例模式
    public static AudioManager instance { get; private set; }

    private AudioSource audioS;

    void Start()
    {
        instance = this;
        audioS = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放指定的音效
    /// </summary>
    /// <param name="clip"></param>
    public void AudioPlay(AudioClip clip)
    {
        audioS.PlayOneShot(clip);
    }
}
