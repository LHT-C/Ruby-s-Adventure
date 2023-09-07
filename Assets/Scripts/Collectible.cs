using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ݮ����Ҽ��ʱ���������
/// </summary>
public class Collectible : MonoBehaviour
{

    public ParticleSystem collectEffect;//ʰȡ��Ч

    public AudioClip collectClip;//ʰȡ��Ч

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// ��ײ������
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc=other.GetComponent<PlayerController>();
        if (pc != null )
        {
            if (pc.MyCurrentHealth < pc.MyMaxHealth)
            {
                pc.ChangeHealth(1);
                Instantiate(collectEffect, transform.position, Quaternion.identity);//������Ч
                AudioManager.instance.AudioPlay(collectClip);//������Ч
                Destroy(this.gameObject);
            }
        }
    }
}
