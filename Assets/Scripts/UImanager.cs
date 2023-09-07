using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI�������
/// </summary>
public class UImanager : MonoBehaviour
{
    //����ģʽ
    public static UImanager instance { get; private set; }

    void Start()
    {
        instance = this;
    }

    public Image healthBar;//��ɫ��Ѫ��

    public Text bulletCountText;//�ӵ�����������ʾ

    /// <summary>
    /// ����Ѫ��
    /// </summary>
    /// <param name="curAmount"></param>
    /// <param name="maxAmount"></param>
    public void UpdateHealthBar(int curAmount,int maxAmount)
    {
        healthBar.fillAmount = (float)curAmount / (float)maxAmount;
    }

    /// <summary>
    ///�����ӵ������ı�����ʾ 
    /// </summary>
    /// <param name="curAmount"></param>
    /// <param name="maxAmount"></param>
    public void UpdateBulletCount(int curAmount,int maxAmount)
    {
        bulletCountText.text = curAmount.ToString() + "/" + maxAmount.ToString();
        //bulletCountText.text = curAmount + "/" + maxAmount;
    }
}
