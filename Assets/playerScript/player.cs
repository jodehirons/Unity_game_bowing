using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Player类用于处理玩家的行为和属性
/// </summary>
public class player : MonoBehaviour
{
    [Header("速度相关")]
    /// <summary>
    /// 玩家的移动速度
    /// </summary>
    public float playerSpeed;
    public float rotateSpeed;

    [Header("控制相关")]
    /// <summary>
    /// 玩家的方向控制
    /// </summary>
    public float rotation;
    public float N1 = 0;
    public float N2 = 0;
    public float aboveRotation = 0;
    public float stopThere = 0;
    public float startThere = 0;

    /// <summary>
    /// 玩家的Y坐标
    /// </summary>
    public float Y_Position;
    public float controlTime;
    [Header("其他相关")]
    /// <summary>
    /// 刚体组件
    /// </summary>
    public Rigidbody2D playerRB;

    /// <summary>
    /// 传送门组件
    /// </summary>
    public Transform playerTransform;

    /// <summary>
    /// 障碍物预制件
    /// </summary>
    public GameObject obstaclePrefab;

    /// <summary>
    /// 掉血音频源
    /// </summary>
    public AudioSource BloodAudiio;

    /// <summary>
    /// 障碍物音频源
    /// </summary>
    public AudioSource ObstacleAudio;
    
    /// <summary>
    /// 在游戏开始时初始化玩家的属性
    /// </summary>
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        playerSpeed = 1f;
        rotateSpeed = 1.5f;
        playerRB.velocity = new Vector2(playerRB.velocity.x, playerSpeed);
        AudioSource[] arr = GetComponents<AudioSource>();
        BloodAudiio = arr[0];
        ObstacleAudio = arr[1];
        Y_Position = playerTransform.position.y;
    }

    /// <summary>
    /// 在每一帧更新玩家状态
    /// </summary>
    void Update()
    {
        adjustStay();
        playerMove();
        AddSpeed();
    }

    /// <summary>
    /// 如果玩家的位置高于之前的位置20个单位，则增加玩家的速度和旋转速度
    /// </summary>
    void AddSpeed()
    {
        if(playerTransform.position.y > Y_Position + 20)
        {
            playerSpeed += 0.05f;
            Y_Position = playerTransform.position.y;
            rotateSpeed += 0.07f;
        }
    }

  
    /// <summary>
    /// 控制玩家方向
    /// </summary>
    /// <param name="horizontal">水平方向的输入值</param>
    void playerMove()
    {
        controlTime+= Time.deltaTime;
        if (controlTime < 0.02f) return;
        controlTime= 0;
        float h1 = Input.GetAxis("HorizontalAB");
        float h2 = Input.GetAxis("Horizontal");
        float horizontal = 0;
        float controlChange = 0.005f;
        if (h1 < 0) horizontal -= controlChange;
        else if(h1 > 0) horizontal += controlChange;
        if (h2 < 0) horizontal -= controlChange;
        else if(h2 > 0) horizontal += controlChange;


        if (horizontal != 0f) 
        {
            
            // 计算船的旋转角度
            rotation = -horizontal * 90 + aboveRotation;
            
            if(rotation < -70)
            {
                rotation = -70;
            }
            else if(rotation > 70)
            {
                rotation = 70;
            }
            aboveRotation = rotation;
            // 计算船的速度和移动方向
            Vector2 direction = new Vector2(Mathf.Cos((rotation+90) * Mathf.Deg2Rad), Mathf.Sin((rotation + 90) * Mathf.Deg2Rad));

            if(stopThere == 0)
                playerRB.velocity = direction * rotateSpeed;
            else playerRB.velocity = direction * rotateSpeed * startThere;
        }
        
        
    }

    /// <summary>
    /// 设置船的旋转角度
    /// </summary>
    private void FixedUpdate()
    {
        
        // 设置船的旋转角度
        playerTransform.rotation = Quaternion.Euler(0, 0, rotation);
        

    }

    /// <summary>
    /// 碰撞判定，当玩家与血包相撞，播放相应音效；与障碍物相撞，播放相应音效并迫使玩家停留一下
    /// </summary>
    /// <param name="collision">碰撞信息</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            ObstacleAudio.Play();
            playerRB.velocity *= 0;
            stopThere = 1;
        }
        if(collision.gameObject.tag == "Blood")
        {
            BloodAudiio.Play();
        }
        
    }

    /// <summary>
    /// 从碰撞后的停留中逐渐恢复速度
    /// </summary>
    void adjustStay()
    {
        if(stopThere != 0)
        {
            startThere += Time.deltaTime;
            playerRB.velocity = new Vector2(playerRB.velocity.x, startThere);
            if(startThere > playerSpeed)
            {
                startThere = 0;
                stopThere = 0;
            }
        }
        
        
    }
}
