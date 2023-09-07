using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� ��Ч
/// </summary>
public class AudioManager : MonoBehaviour
{
    //����ģʽ
    public static AudioManager instance { get; private set; }

    private AudioSource audioS;

    void Start()
    {
        instance = this;
        audioS = GetComponent<AudioSource>();
    }

    /// <summary>
    /// ����ָ������Ч
    /// </summary>
    /// <param name="clip"></param>
    public void AudioPlay(AudioClip clip)
    {
        audioS.PlayOneShot(clip);
    }
}
