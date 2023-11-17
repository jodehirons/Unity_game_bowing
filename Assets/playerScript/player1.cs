using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1 : MonoBehaviour
{
    /// <summary>
    /// ¿ØÖÆÍæ¼Ò1µÄ¶¯»­ÇÐ»»
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

    //Íæ¼Ò»®´¬
    void boating()
    {
        player1Anim.SetBool("boat1", true);


        //»Ö¸´idle
        player1Anim.SetBool("boat1", false);
    }

    //Íæ¼Òµ¹ÏÂ
    void fall()
    {
        player1Anim.SetBool("fall1", true);


        //»Ö¸´idle
        player1Anim.SetBool("fall1", false);
    }

    //Íæ¼Ò¾ÈÈË
    void save()
    {
        player1Anim.SetBool("save1", true);


        //»Ö¸´idle
        player1Anim.SetBool("save1", false);
    }

    //Íæ¼ÒÇÐÇ¹ÇÐ½¬
    void change()
    {
        player1Anim.SetBool("change1", true);


        //»Ö¸´idle
        player1Anim.SetBool("change1", false);
    }
}
