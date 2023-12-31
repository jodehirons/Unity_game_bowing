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
    public float controlDong;
    public float controlKe;
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
    public AudioSource[] myAudios;
    public CapsuleCollider2D playerCollider;
    public TextMeshProUGUI SkillMouse;
    public TextMeshProUGUI DiedText;
    public TextMeshProUGUI ScoreText;
    public GameObject gameoverPage;
    public TMP_Text score;
    public Animator playerLeft;
    public Animator playerRight;
    public Animator playerColl;
    public Transform PeopleLeft;
    public TMP_Text pauseText;
    public Slider mainSlider;
    public Slider effectSlider;
    void OnEnable()
    {
        Time.timeScale = 1;
        gameoverPage.SetActive(false);
        playerRB = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        playerSpeed = 1f;
        rotateSpeed = 1.5f;
        playerRB.velocity = new Vector2(playerRB.velocity.x, playerSpeed);
        myAudios = GetComponents<AudioSource>();
        BloodAudiio = myAudios[0];
        ObstacleAudio = myAudios[1];
        Y_Position = playerTransform.position.y;
        playerCollider = GetComponent<CapsuleCollider2D>();
        abovePositon = -7;
    }


    void Update()
    {
        if (DiedText.text == "0")
        {
            ScoreText.text = "积分：" + Mathf.Floor(controlScore);
            Time.timeScale = 0;
            pauseText.text = "彩笔，你们寄了\r\n最终获得的积分是：";
            gameoverPage.SetActive(true);
            PlayerPrefs.SetInt("score", (int)controlScore);
            score.text = Mathf.Floor(controlScore).ToString();
            //结束
        }
        controlMove += Time.deltaTime;
        if (controlMove > 0.1f)
        {
            if (playerTransform.position.y - abovePositon >= 10)
            {
                controlScore += playerTransform.position.y - abovePositon;
                abovePositon = playerTransform.position.y;
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
            if (controlDong != 0)
            {

                if (controlDong == 0.01f)
                {
                    PeopleLeft.rotation = Quaternion.Euler(0f, 0f, 90f);
                    playerLeft.SetBool("p3", true);
                    rotateSpeed /= 1.5f;
                    playerSpeed /= 1.5f;
                    playerTransform.localScale *= 1.3f;
                }
                else if (controlDong > 2)
                {
                    playerLeft.SetBool("p3", false);
                    PeopleLeft.rotation = Quaternion.Euler(0f, 0f, -130f);
                }
                playerRB.velocity = new Vector3(playerRB.velocity.x, playerSpeed);
                //playerCollider.isTrigger = true;
                controlDong += Time.deltaTime;
                if (controlDong > 20)
                {
                    controlDong = 0;
                    rotateSpeed *= 1.5f;
                    playerSpeed *= 1.5f;
                    playerRB.velocity = new Vector3(0, playerSpeed);
                    playerTransform.localScale /= 1.3f;
                    //playerCollider.isTrigger = false;
                }
            }
            else if (controlKe != 0)
            {
                if (controlKe == 0.01f)
                {
                    playerLeft.SetBool("p3", true);
                    PeopleLeft.rotation = Quaternion.Euler(0f, 0f, 90f);
                    playerTransform.localScale /= 1.5f;
                    rotateSpeed *= 2.5f;
                    playerSpeed *= 2.5f;
                }
                else if (controlKe > 2)
                {
                    playerLeft.SetBool("p3", false);
                    PeopleLeft.rotation = Quaternion.Euler(0f, 0f, -130f);
                }
                controlKe += Time.deltaTime;
                playerRB.velocity = new Vector3(playerRB.velocity.x, playerSpeed);
                if (controlKe > 20)
                {
                    controlKe = 0;
                    playerTransform.localScale *= 1.5f;
                    rotateSpeed /= 2.5f;
                    playerSpeed /= 2.5f;
                    playerRB.velocity = new Vector3(0, playerSpeed);
                }
            }
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
            if (controlRecover > 1)
            {
                playerCollider.isTrigger = false;
                controlRecover = 0;
            }
        }

        if (shipOut.y != 0 && playerTransform.position.y >= shipOut.y - 0.1f)
        {
            controlCollider = 0;
            controlRecover = 0.01f;
            shipOut = Vector3.zero;
        }
        // 键盘按p暂停
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                pauseText.text = "你们目前的得分为：";
                gameoverPage.SetActive(true);
                PlayerPrefs.SetInt("score", (int)controlScore);
                score.text = Mathf.Floor(controlScore).ToString();
            }
            else
            {
                Time.timeScale = 1;
                gameoverPage.SetActive(false);
            }
        }
        // music control
        // 根据滑动条的值来设置音量
        if (effectSlider != null)
        {
            BloodAudiio.volume = mainSlider.value * effectSlider.value;
            ObstacleAudio.volume = mainSlider.value * effectSlider.value;
            boatingAudio.volume = mainSlider.value * effectSlider.value;
            goldAudio.volume = mainSlider.value * effectSlider.value;
        }
    }

    public void gameAgain()
    {
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
            if (shipOut.y != 0)
            {
                controlCollider = 1f;

            }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "GoldCoin")
        {
            goldAudio.Play();
            controlScore += 10;
        }
        if (collision.gameObject.tag == "dongdong")
        {
            //Destroy(collision.gameObject);
            controlDong = 0.01f;
            controlScore += 20;
        }
        if (collision.gameObject.tag == "keke")
        {
            //Destroy(collision.gameObject);
            controlKe = 0.01f;
            controlScore += 20;
        }
    }

    void getMouse()
    {
        if (Time.timeScale != 0)
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
                SkillMouse.text = "鼠标：" + temp.ToString();
                if (controlMouse >= 10f)
                {
                    SkillMouse.text = "鼠标：可用";
                    controlMouse = 0;
                }
            }
        }
    }

    void AddSpeed()
    {
        if (playerTransform.position.y > Y_Position + 20)
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
        else if (h1 > 0) horizontal += controlChange;
        if (h2 < 0) horizontal -= controlChange;
        else if (h2 > 0) horizontal += controlChange;

        if (h1 < 0)
        {
            playerLeft.SetBool("p1", true);
            playerLeft.SetBool("mirror", false);
        }
        else if (h1 > 0)
        {
            playerLeft.SetBool("p1", false);
            playerLeft.SetBool("mirror", true);
        }
        else
        {
            playerLeft.SetBool("p1", false);
            playerLeft.SetBool("mirror", false);
        }
        if (h2 > 0)
        {
            playerRight.SetBool("p1", true);
            playerRight.SetBool("mirror", false);
        }
        else if (h2 < 0)
        {
            playerRight.SetBool("mirror", true);
            playerRight.SetBool("p1", false);
        }
        else
        {
            playerRight.SetBool("p1", false);
            playerRight.SetBool("mirror", false);
        }

        if ((h1 != 0 || h2 != 0) && !boatingAudio.isPlaying)
        {
            boatingAudio.Play();
        }
        if (h1 == 0 && h2 == 0)
        {
            boatingAudio.Stop();
        }


        controlTime += Time.deltaTime;
        if (controlTime < 0.02f) return;
        controlTime = 0;

        if (horizontal != 0f || rotation != 0)
        {

            // 计算船的旋转角度
            rotation = -horizontal * 90 + aboveRotation;

            if (rotation < -70)
            {
                rotation = -70;
            }
            else if (rotation > 70)
            {
                rotation = 70;
            }
            aboveRotation = rotation;
            // 计算船的速度和移动方向
            Vector2 direction = new Vector2(Mathf.Cos((rotation + 90) * Mathf.Deg2Rad), Mathf.Sin((rotation + 90) * Mathf.Deg2Rad));


            if (stopThere == 0)
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
        if (stopThere != 0)
        {
            startThere += Time.deltaTime;
            playerRB.velocity = new Vector2(playerRB.velocity.x, startThere * playerSpeed);
            if (startThere > 1)
            {
                startThere = 0;
                stopThere = 0;
                playerColl.SetBool("p1", false);
            }
        }


    }
}
