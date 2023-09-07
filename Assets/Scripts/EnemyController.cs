using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人控制相关
/// </summary>
public class EnemyController : MonoBehaviour
{
    public float speed = 3f;//移动速度

    public float changeDirectionTime = 2f;//改变方向的时间
    private float changeTimer;//改变方向的计时器
    public bool isVertical;//是否是垂直方向移动
    private Vector2 moveDirection;//移动方向

    public ParticleSystem brokenEffect;//损坏特效
    public AudioClip fixedClip;//修复音效

    private bool isFixed;//是否被修复

    private Rigidbody2D rbody;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        moveDirection = isVertical ? Vector2.up : Vector2.right;//如果是垂直移动，方向就朝上；否则方向朝右

        changeTimer = changeDirectionTime;

        isFixed = false;//默认未修理
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFixed)
        {
            //Destroy(this.gameObject);//修复后消失
            return;//如果被修复，则不会执行下面的代码
        }

        changeTimer -= Time.deltaTime; 
        if (changeTimer < 0)
        {
            moveDirection*=-1;
            changeTimer = changeDirectionTime;
        }

        Vector2 position = rbody.position;
        position.x += moveDirection.x * speed * Time.deltaTime;
        position.y += moveDirection.y * speed * Time.deltaTime;
        rbody.MovePosition(position);
        anim.SetFloat("moveX", moveDirection.x);//moveX是unity里面animator设置中的parameters
        anim.SetFloat("moveY", moveDirection.y);
    }

    /// <summary>
    /// 与玩家的碰撞检测
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerController pc=other.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.ChangeHealth(-1);
        }
    }

    /// <summary>
    /// 敌人被修复
    /// </summary>
    public void Fixed()
    {
        isFixed = true;
        if(brokenEffect.isPlaying==true)
        {
            brokenEffect.Stop();//停止播放损坏动画
        }
        AudioManager.instance.AudioPlay(fixedClip);//播放修复音效
        rbody.simulated = false;//禁用物理
        anim.SetTrigger("fix");//播放被修复的动画
    }
}
