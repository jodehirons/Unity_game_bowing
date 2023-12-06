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

    private void Start()
    {
        Help = GetComponents<AudioSource>();
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
            if(isPlaying && player.position.y > 10)
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
            if(isPaused && player.position.y > 10)
            {
                Help[0].Stop();
                Help[1].Play();
                Help[2].Play();
                isPaused = false;
            }
            float distance = Vector3.Distance(player.position, transform.position);
            float volume = Mathf.Clamp01(5f / (distance * 1f));
            Help[1].volume = volume;
            Help[2].volume = volume;
        }

        if(player.position.y > transform.position.y + 10)
        {
            Help[1].Stop();
            Help[2].Stop();
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