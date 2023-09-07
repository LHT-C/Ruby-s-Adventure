using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��ҩ��
/// </summary>
public class BulletBag : MonoBehaviour
{
    public int bulletCount = 30;//������ѵ��ӵ�����

    public ParticleSystem collectEffect;//ʰȡ��Ч

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc=other.GetComponent<PlayerController>();
        if (pc != null)
        {
            if (pc.MyCurBulletCount<pc.MyMaxBulletCount)
            {
                pc.ChangeBulletCount(bulletCount);
                Instantiate(collectEffect,transform.position, Quaternion.identity);//���ʧȥ��Ч
                Destroy(this.gameObject);
            }
        }
    }
}
