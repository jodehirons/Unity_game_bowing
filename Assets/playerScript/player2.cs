using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player2 : MonoBehaviour
{
    /// <summary>
    /// ¿ØÖÆÍæ¼Ò2µÄ¶¯»­ÇÐ»»
    /// </summary>
    Animator player2Anim;

    // Start is called before the first frame update
    void Start()
    {
        player2Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Íæ¼Òµ¹ÏÂ
    void fall()
    {
        player2Anim.SetBool("fall2", true);


        //»Ö¸´»®´¬
        player2Anim.SetBool("fall2", false);
    }

    //Íæ¼Ò¾ÈÈË
    void save()
    {
        player2Anim.SetBool("save2", true);


        //»Ö¸´»®´¬
        player2Anim.SetBool("save2", false);
    }

    //Íæ¼ÒÇÐÇ¹ÇÐ½¬
    void change()
    {
        player2Anim.SetBool("change2", true);


        //»Ö¸´»®´¬
        player2Anim.SetBool("change2", false);
    }
}
