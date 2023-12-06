using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class buff : MonoBehaviour
{
    public AudioSource[] Help;
    public Transform player;
    public bool isPlaying = true;
    public bool isPaused = true;
    public float control1;

    private void Start()
    {
        Help = GetComponents<AudioSource>();
        control1= 0;
    }

    void Update()
    {
        //if (transform.position.y + 20 < player.position.y)
        //{
        //    Destroy(gameObject);
        //}

        //// 获取当前游戏对象到音频源的距离  
        //float distance = Vector3.Distance(transform.position, audioSource.transform.position);

        //// 根据距离调整音量  
        //float volume = Mathf.Clamp01(maxDistance / (distance * volumeStep));
        //audioSource.volume = volume;
        if (player.position.y + 8 > transform.position.y)
        {
            if(isPlaying && transform.position.y > 10)
            {
                Help[0].Play();
                isPlaying = false;
            }
            float distance = Vector3.Distance(player.position, transform.position);
            float volume = Mathf.Clamp01(5f / (distance * 1f));
            Help[0].volume = volume;
        }


        if(player.position.y > transform.position.y)
        {
            if(isPaused && transform.position.y > 10)
            {
                Help[0].Stop();
                Help[2].Play();
                Help[3].Play();
                isPaused = false;
                control1 = 0.01f;
            }
        }

        if (control1 != 0)
        {
            control1 += Time.deltaTime;

            if (control1 > 3 && control1 < 3.1f && !Help[1].isPlaying)
            {
                Help[1].Play();
            }
            if(control1 > 10 && transform.position.y > 10)
            {
                Destroy(gameObject);
            }
        }

        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            Destroy(gameObject);
        }
    }

   
}