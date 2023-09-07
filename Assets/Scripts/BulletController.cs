using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����ӵ����ƶ�����ײ
/// </summary>
public class BulletController : MonoBehaviour
{
    Rigidbody2D rbody;

    public AudioClip hitCilp;//������Ч

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
    /// �ӵ����ƶ�
    /// </summary>
    /// <param name="moveDirection"></param>
    /// <param name="moveForce"></param>
    public void Move(Vector2 moveDirection,float moveForce)
    {
        rbody.AddForce(moveDirection * moveForce);
    }

    //��ײ���
    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController ec=other.gameObject.GetComponent<EnemyController>();
        if (ec != null)
        {
            ec.Fixed();//�޸�����
            Debug.Log("���е��ˣ�");
        }
        AudioManager.instance.AudioPlay(hitCilp);//����������Ч
        Destroy(this.gameObject);
    }
}
