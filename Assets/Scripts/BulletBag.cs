using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 弹药包
/// </summary>
public class BulletBag : MonoBehaviour
{
    public int bulletCount = 30;//包里好友的子弹数量

    public ParticleSystem collectEffect;//拾取特效

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc=other.GetComponent<PlayerController>();
        if (pc != null)
        {
            if (pc.MyCurBulletCount<pc.MyMaxBulletCount)
            {
                pc.ChangeBulletCount(bulletCount);
                Instantiate(collectEffect,transform.position, Quaternion.identity);//添加失去特效
                Destroy(this.gameObject);
            }
        }
    }
}
