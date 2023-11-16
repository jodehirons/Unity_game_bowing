using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class player : MonoBehaviour
{
    [Header("速度相关")]
    public float playerSpeed;
    public float rotateSpeed;
    [Header("控制相关")]
    public float rotation;
    public float N1 = 0;
    public float N2 = 0;
    public float aboveRotation = 0;
    public float stopThere = 0;
    public float startThere = 0;
    public float Y_Position;
    public float controlTime;
    public float controlCollider;
    public float controlMouse;
    public float controlRecover;
    [Header("位置相关")]
    public Vector3 shipOut;
    [Header("其他相关")]
    public Rigidbody2D playerRB;
    public Transform playerTransform;
    public GameObject portal;
    public AudioSource BloodAudiio;
    public AudioSource ObstacleAudio;
    public CapsuleCollider2D playerCollider;
    public TextMeshProUGUI SkillMouse;
    public TextMeshProUGUI DiedText;


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
        playerCollider = GetComponent<CapsuleCollider2D>();
        
    }

    
    void Update()
    {
        if(DiedText.text == "0")
        {
            //结束
        }
        if (controlCollider == 0)
        {
            adjustStay();
            playerMove();
            AddSpeed();
            getMouse();
        }
        else
        {
            Vector2 a = new Vector2(shipOut.x - playerTransform.position.x, shipOut.y - playerTransform.position.y);
            playerRB.velocity = a;
            playerCollider.isTrigger = true;
        }
        if (controlRecover != 0)
        {
            playerRB.velocity = new Vector2(0, playerSpeed);
            controlRecover += Time.deltaTime;
            if(controlRecover > 2)
            {
                playerCollider.isTrigger = false;
                controlRecover= 0;
            }
        }
        
        if(shipOut.y != 0 && playerTransform.position.y >= shipOut.y-0.1f)
        {
            controlCollider = 0;
            controlRecover = 0.01f;
            shipOut = Vector3.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            ObstacleAudio.Play();
            playerRB.velocity *= 0;
            stopThere = 1;
        }
        if (collision.gameObject.tag == "Blood")
        {
            BloodAudiio.Play();
       
        }
        if (collision.gameObject.tag == "portal")
        {
            if(shipOut.y != 0)
            {
                controlCollider = 1f;
          
            }
            Destroy(collision.gameObject);
        }

    }

    
    
    void getMouse()
    {
        Vector3 clickPosition = new Vector3();
        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Input.mousePosition;  // 获取鼠标点击位置的坐标  
            clickPosition = Camera.main.ScreenToWorldPoint(clickPosition);  // 如果需要，把鼠标屏幕坐标转换成世界坐标  
            if (controlMouse == 0)
            {
                shipOut = clickPosition;
                Debug.Log(shipOut);
                controlMouse = 0.01f;
                Instantiate(portal, new Vector3(shipOut.x, shipOut.y, 0), Quaternion.identity);
            }
        }
        
        if (controlMouse != 0)
        {
            controlMouse += Time.deltaTime;
            float temp = Mathf.Ceil(10f - controlMouse);
            SkillMouse.text = "鼠标："+ temp.ToString();
            if (controlMouse >= 10f)
            {
                SkillMouse.text = "鼠标：可用";
                controlMouse = 0;
            }
        }
        
    }

    void AddSpeed()
    {
        if(playerTransform.position.y > Y_Position + 20)
        {
            playerSpeed += 0.05f;
            Y_Position = playerTransform.position.y;
            rotateSpeed += 0.07f;
        }
    }

  

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


        if (horizontal != 0f || rotation != 0) 
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
    private void FixedUpdate()
    {
        
        // 设置船的旋转角度
        playerTransform.rotation = Quaternion.Euler(0, 0, rotation);
        

    }

    

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
