using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1 : MonoBehaviour
{
    /// <summary>
    /// �������1�Ķ����л�
    /// </summary>
    Animator player1Anim;


    // Start is called before the first frame update
    void Start()
    {
        player1Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //��ҵ���
    void fall()
    {
        player1Anim.SetBool("fall1", true);


        //�ָ�����
        player1Anim.SetBool("fall1", false);
    }

    //��Ҿ���
    void save()
    {
        player1Anim.SetBool("save1", true);


        //�ָ�����
        player1Anim.SetBool("save1", false);
    }

    //�����ǹ�н�
    void change()
    {
        player1Anim.SetBool("change1", true);


        //�ָ�����
        player1Anim.SetBool("change1", false);
    }
}
