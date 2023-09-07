using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 控制角色移动、生命、动画等
/// </summary>
public class PlayerController : MonoBehaviour 
{
    public float speed = 5f;//移动速度

    private int maxHealth = 5;//最大生命值
    private int currentHealth;//当前生命值

    [SerializeField]
    private int maxBulletCount = 99;//最大子啊但数量
    private int curBulletCount;//当前子弹数量

    public int MyCurBulletCount { get { return curBulletCount; } }
    public int MyMaxBulletCount { get { return maxBulletCount; } }

    public int MyMaxHealth { get { return maxHealth; } }
    public int MyCurrentHealth { get { return currentHealth; } }

    private float invincibleTime = 2f;//无敌时间2s
    private float invincibleTimer;//无敌计时器
    private bool isInvincible;//是否处于无敌状态

    public GameObject bulletPrefab;//子弹

    public AudioClip hitClip;//受伤音效
    public AudioClip launchClip;//发射齿轮音效

    private Vector2 lookDirection = new Vector2(0, 1);//默认朝向下方

    private Rigidbody2D rbody;//刚体组件

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //currentHealth = maxHealth;
        currentHealth = 3;
        UImanager.instance.UpdateHealthBar(currentHealth, maxHealth);//开局血条不满的情况下，更新血条
        
        curBulletCount = 20;
        UImanager.instance.UpdateBulletCount(curBulletCount, maxBulletCount);
        
        invincibleTimer = 0;//开局不是无敌状态
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.Translate(transform.right * speed * Time.deltaTime);//测试移动

        float moveX = Input.GetAxisRaw("Horizontal");//控制水平移动方向 A:-1 D:1 0
        float moveY = Input.GetAxisRaw("Vertical");//控制垂直移动方向 W:1 S:-1 0

        //动画
        Vector2 moveVector = new Vector2(moveX, moveY);
        if(moveVector.x!=0||moveVector.y!=0)
        {
            lookDirection = moveVector;
        }
        anim.SetFloat("Look X", lookDirection.x);
        anim.SetFloat("Look Y", lookDirection.y);
        anim.SetFloat("Speed", moveVector.magnitude);

        //移动
        //Vector2 position = transform.position;
        Vector2 position = rbody.position;//刚体移动
        //position.x += moveX * speed * Time.deltaTime;
        //position.y += moveY * speed * Time.deltaTime;
        position += moveVector * speed * Time.fixedDeltaTime;//直接使用向量(角色朝向)
        //transform.position = position;
        rbody.MovePosition(position);

        if(isInvincible)//无敌计时
        {
            invincibleTimer -= Time.fixedDeltaTime;
            if(invincibleTimer < 0 )
            {
                isInvincible = false;//倒计时2秒结束后，取消无敌状态
            }
        }

        
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.J))//按下J键进行攻击
        if (Input.GetMouseButtonDown(0)&&curBulletCount>0)//按下鼠标左键进行攻击
        {
            ChangeBulletCount(-1);//每次攻击子弹减一
            anim.SetTrigger("Launch");//播放攻击动画
            AudioManager.instance.AudioPlay(launchClip);//播放攻击音效
            GameObject bullet = Instantiate(bulletPrefab, rbody.position+Vector2.up*0.5f, Quaternion.identity);//子弹出现的位置与角色一致
            BulletController bc=bullet.GetComponent<BulletController>();
            if(bc != null)
            {
                bc.Move(lookDirection, 600);//子弹方向和速度
            }
        }

        if (Input.GetKeyDown(KeyCode.E))//按下e键互动
        {
            RaycastHit2D hit = Physics2D.Raycast(rbody.position, lookDirection, 1.5f, LayerMask.GetMask("NPC"));//发出射线，碰撞到npc层级上的box后返回
            if (hit.collider != null)
            {
                NPCmanager npc=hit.collider.GetComponent<NPCmanager>();
                if(npc != null)
                {
                    npc.ShowDialog();//显示对话框
                    Debug.Log("hit npc!");
                }
                
            }
        }
    }

    /// <summary>
    /// 改变玩家的生命值
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHealth(int amount)
    {
        if (amount < 0)//玩家收到伤害时
        {
            if(isInvincible==true)
            {
                return;
            }
            isInvincible = true;
            anim.SetTrigger("Hit");
            AudioManager.instance.AudioPlay(hitClip);//播放受伤音效
            invincibleTimer = invincibleTime;
        }


        Debug.Log(currentHealth + "/" + maxHealth);

        //把玩家的生命值约束在0和最大值之间
        currentHealth = math.clamp(currentHealth + amount, 0, maxHealth);
        UImanager.instance.UpdateHealthBar(currentHealth, maxHealth);//更新血条
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    /// <summary>
    /// 改变子弹数量
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeBulletCount(int amount)
    {
        curBulletCount = Mathf.Clamp(curBulletCount + amount, 0, maxBulletCount);//限制子弹数量在0到最大值之间
        UImanager.instance.UpdateBulletCount(curBulletCount, maxBulletCount);
    }
}
