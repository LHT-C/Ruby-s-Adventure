using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制子弹的移动、碰撞
/// </summary>
public class BulletController : MonoBehaviour
{
    Rigidbody2D rbody;

    public AudioClip hitCilp;//命中音效

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 子弹的移动
    /// </summary>
    /// <param name="moveDirection"></param>
    /// <param name="moveForce"></param>
    public void Move(Vector2 moveDirection,float moveForce)
    {
        rbody.AddForce(moveDirection * moveForce);
    }

    //碰撞检测
    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController ec=other.gameObject.GetComponent<EnemyController>();
        if (ec != null)
        {
            ec.Fixed();//修复敌人
            Debug.Log("击中敌人！");
        }
        AudioManager.instance.AudioPlay(hitCilp);//播放命中音效
        Destroy(this.gameObject);
    }
}
