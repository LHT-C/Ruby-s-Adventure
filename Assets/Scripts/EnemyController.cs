using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���˿������
/// </summary>
public class EnemyController : MonoBehaviour
{
    public float speed = 3f;//�ƶ��ٶ�

    public float changeDirectionTime = 2f;//�ı䷽���ʱ��
    private float changeTimer;//�ı䷽��ļ�ʱ��
    public bool isVertical;//�Ƿ��Ǵ�ֱ�����ƶ�
    private Vector2 moveDirection;//�ƶ�����

    public ParticleSystem brokenEffect;//����Ч
    public AudioClip fixedClip;//�޸���Ч

    private bool isFixed;//�Ƿ��޸�

    private Rigidbody2D rbody;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        moveDirection = isVertical ? Vector2.up : Vector2.right;//����Ǵ�ֱ�ƶ�������ͳ��ϣ���������

        changeTimer = changeDirectionTime;

        isFixed = false;//Ĭ��δ����
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFixed)
        {
            //Destroy(this.gameObject);//�޸�����ʧ
            return;//������޸����򲻻�ִ������Ĵ���
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
        anim.SetFloat("moveX", moveDirection.x);//moveX��unity����animator�����е�parameters
        anim.SetFloat("moveY", moveDirection.y);
    }

    /// <summary>
    /// ����ҵ���ײ���
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
    /// ���˱��޸�
    /// </summary>
    public void Fixed()
    {
        isFixed = true;
        if(brokenEffect.isPlaying==true)
        {
            brokenEffect.Stop();//ֹͣ�����𻵶���
        }
        AudioManager.instance.AudioPlay(fixedClip);//�����޸���Ч
        rbody.simulated = false;//��������
        anim.SetTrigger("fix");//���ű��޸��Ķ���
    }
}
