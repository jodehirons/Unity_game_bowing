using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class player : MonoBehaviour
{
    [Header("�ٶ����")]
    public float playerSpeed;
    public float rotateSpeed;
    [Header("�������")]
    public float rotation;
    public float N1 = 0;
    public float N2 = 0;
    public float aboveRotation = 0;
    public float stopThere = 0;
    public float startThere = 0;
    public float Y_Position;
    [Header("�������")]
    public Rigidbody2D playerRB;
    public Transform playerTransform;
    public GameObject obstaclePrefab;
    public AudioSource BloodAudiio;
    public AudioSource ObstacleAudio;
    

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

    
    void Update()
    {
        adjustStay();
        playerMove();
        AddSpeed();
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
        float h1 = Input.GetAxis("HorizontalAB");
        float h2 = Input.GetAxis("Horizontal");
        float horizontal = 0;
        float controlChange = 0.002f;
        if (h1 < 0) horizontal -= controlChange;
        else if(h1 > 0) horizontal += controlChange;
        if (h2 < 0) horizontal -= controlChange;
        else if(h2 > 0) horizontal += controlChange;


        if (horizontal != 0f) 
        {
            
            // ���㴬����ת�Ƕ�
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
            // ���㴬���ٶȺ��ƶ�����
            Vector2 direction = new Vector2(Mathf.Cos((rotation+90) * Mathf.Deg2Rad), Mathf.Sin((rotation + 90) * Mathf.Deg2Rad));

            if(stopThere == 0)
                playerRB.velocity = direction * rotateSpeed;
            else playerRB.velocity = direction * rotateSpeed * startThere;
        }
        
        
    }
    private void FixedUpdate()
    {
        
        // ���ô�����ת�Ƕ�
        playerTransform.rotation = Quaternion.Euler(0, 0, rotation);
        

    }

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