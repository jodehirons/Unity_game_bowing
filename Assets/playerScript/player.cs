using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public float controlMove;
    public float controlScore;
    public float abovePositon;
    [Header("位置相关")]
    public Vector3 shipOut;
    [Header("其他相关")]
    public Rigidbody2D playerRB;
    public Transform playerTransform;
    public GameObject portal;
    public AudioSource BloodAudiio;
    public AudioSource ObstacleAudio;
    public AudioSource boatingAudio;
    public AudioSource goldAudio;
    public CapsuleCollider2D playerCollider;
    public TextMeshProUGUI SkillMouse;
    public TextMeshProUGUI DiedText;
    public TextMeshProUGUI ScoreText;
    public GameObject gameoverPage;
    public TMP_Text score;
    public Animator playerLeft;
    public Animator playerRight;
    public Animator playerColl;
    void OnEnable()
    {
        gameoverPage.SetActive(false);
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
        abovePositon = -7;
    }

    
    void Update()
    {
        if(DiedText.text == "0")
        {
            Time.timeScale = 0;
            gameoverPage.SetActive(true);
            PlayerPrefs.SetInt("score", (int)controlScore);
            score.text = Mathf.Floor(controlScore).ToString();
            //结束
        }
        controlMove += Time.deltaTime;
        if(controlMove > 0.1f)
        {
            if(playerTransform.position.y - abovePositon >= 10)
            {
                controlScore += playerTransform.position.y - abovePositon;
                abovePositon= playerTransform.position.y;
            }
            
            ScoreText.text = "积分：" + Mathf.Floor(controlScore);
            controlMove = 0;
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
            if(controlRecover > 1)
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

    public void gameAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "GunShoot")
        {
            ObstacleAudio.Play();
            playerRB.velocity *= 0;
            stopThere = 1;
            controlScore -= 10;
            playerColl.SetBool("p1", true);
        }
        if (collision.gameObject.tag == "Blood")
        {
            BloodAudiio.Play();
            controlScore += 10;
        }
        if (collision.gameObject.tag == "portal")
        {
            if(shipOut.y != 0)
            {
                controlCollider = 1f;
          
            }
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "GoldCoin")
        {
            goldAudio.Play();
            controlScore+= 10;
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
        
        float h1 = Input.GetAxisRaw("HorizontalAB");
        float h2 = Input.GetAxisRaw("Horizontal");
        float horizontal = 0;
        float controlChange = 0.005f;
        if (h1 < 0) horizontal -= controlChange;
        else if(h1 > 0) horizontal += controlChange;
        if (h2 < 0) horizontal -= controlChange;
        else if(h2 > 0) horizontal += controlChange;

        if (h1 != 0) playerLeft.SetBool("p1", true);
        else playerLeft.SetBool("p1", false);
        if (h2 != 0) playerRight.SetBool("p1", true);
        else playerRight.SetBool("p1", false);

        if(h1 != 0 || h2 != 0)
        {
            boatingAudio.Play();
        }

        controlTime += Time.deltaTime;
        if (controlTime < 0.02f) return;
        controlTime = 0;

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
            playerRB.velocity = new Vector2(playerRB.velocity.x, startThere/2*playerSpeed);
            if(startThere > 1)
            {
                startThere = 0;
                stopThere = 0;
                playerColl.SetBool("p1", false);
            }
        }
        
        
    }
}
