using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// ���ƽ�ɫ�ƶ���������������
/// </summary>
public class PlayerController : MonoBehaviour 
{
    public float speed = 5f;//�ƶ��ٶ�

    private int maxHealth = 5;//�������ֵ
    private int currentHealth;//��ǰ����ֵ

    [SerializeField]
    private int maxBulletCount = 99;//����Ӱ�������
    private int curBulletCount;//��ǰ�ӵ�����

    public int MyCurBulletCount { get { return curBulletCount; } }
    public int MyMaxBulletCount { get { return maxBulletCount; } }

    public int MyMaxHealth { get { return maxHealth; } }
    public int MyCurrentHealth { get { return currentHealth; } }

    private float invincibleTime = 2f;//�޵�ʱ��2s
    private float invincibleTimer;//�޵м�ʱ��
    private bool isInvincible;//�Ƿ����޵�״̬

    public GameObject bulletPrefab;//�ӵ�

    public AudioClip hitClip;//������Ч
    public AudioClip launchClip;//���������Ч

    private Vector2 lookDirection = new Vector2(0, 1);//Ĭ�ϳ����·�

    private Rigidbody2D rbody;//�������

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //currentHealth = maxHealth;
        currentHealth = 3;
        UImanager.instance.UpdateHealthBar(currentHealth, maxHealth);//����Ѫ������������£�����Ѫ��
        
        curBulletCount = 20;
        UImanager.instance.UpdateBulletCount(curBulletCount, maxBulletCount);
        
        invincibleTimer = 0;//���ֲ����޵�״̬
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.Translate(transform.right * speed * Time.deltaTime);//�����ƶ�

        float moveX = Input.GetAxisRaw("Horizontal");//����ˮƽ�ƶ����� A:-1 D:1 0
        float moveY = Input.GetAxisRaw("Vertical");//���ƴ�ֱ�ƶ����� W:1 S:-1 0

        //����
        Vector2 moveVector = new Vector2(moveX, moveY);
        if(moveVector.x!=0||moveVector.y!=0)
        {
            lookDirection = moveVector;
        }
        anim.SetFloat("Look X", lookDirection.x);
        anim.SetFloat("Look Y", lookDirection.y);
        anim.SetFloat("Speed", moveVector.magnitude);

        //�ƶ�
        //Vector2 position = transform.position;
        Vector2 position = rbody.position;//�����ƶ�
        //position.x += moveX * speed * Time.deltaTime;
        //position.y += moveY * speed * Time.deltaTime;
        position += moveVector * speed * Time.fixedDeltaTime;//ֱ��ʹ������(��ɫ����)
        //transform.position = position;
        rbody.MovePosition(position);

        if(isInvincible)//�޵м�ʱ
        {
            invincibleTimer -= Time.fixedDeltaTime;
            if(invincibleTimer < 0 )
            {
                isInvincible = false;//����ʱ2�������ȡ���޵�״̬
            }
        }

        
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.J))//����J�����й���
        if (Input.GetMouseButtonDown(0)&&curBulletCount>0)//�������������й���
        {
            ChangeBulletCount(-1);//ÿ�ι����ӵ���һ
            anim.SetTrigger("Launch");//���Ź�������
            AudioManager.instance.AudioPlay(launchClip);//���Ź�����Ч
            GameObject bullet = Instantiate(bulletPrefab, rbody.position+Vector2.up*0.5f, Quaternion.identity);//�ӵ����ֵ�λ�����ɫһ��
            BulletController bc=bullet.GetComponent<BulletController>();
            if(bc != null)
            {
                bc.Move(lookDirection, 600);//�ӵ�������ٶ�
            }
        }

        if (Input.GetKeyDown(KeyCode.E))//����e������
        {
            RaycastHit2D hit = Physics2D.Raycast(rbody.position, lookDirection, 1.5f, LayerMask.GetMask("NPC"));//�������ߣ���ײ��npc�㼶�ϵ�box�󷵻�
            if (hit.collider != null)
            {
                NPCmanager npc=hit.collider.GetComponent<NPCmanager>();
                if(npc != null)
                {
                    npc.ShowDialog();//��ʾ�Ի���
                    Debug.Log("hit npc!");
                }
                
            }
        }
    }

    /// <summary>
    /// �ı���ҵ�����ֵ
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHealth(int amount)
    {
        if (amount < 0)//����յ��˺�ʱ
        {
            if(isInvincible==true)
            {
                return;
            }
            isInvincible = true;
            anim.SetTrigger("Hit");
            AudioManager.instance.AudioPlay(hitClip);//����������Ч
            invincibleTimer = invincibleTime;
        }


        Debug.Log(currentHealth + "/" + maxHealth);

        //����ҵ�����ֵԼ����0�����ֵ֮��
        currentHealth = math.clamp(currentHealth + amount, 0, maxHealth);
        UImanager.instance.UpdateHealthBar(currentHealth, maxHealth);//����Ѫ��
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    /// <summary>
    /// �ı��ӵ�����
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeBulletCount(int amount)
    {
        curBulletCount = Mathf.Clamp(curBulletCount + amount, 0, maxBulletCount);//�����ӵ�������0�����ֵ֮��
        UImanager.instance.UpdateBulletCount(curBulletCount, maxBulletCount);
    }
}
