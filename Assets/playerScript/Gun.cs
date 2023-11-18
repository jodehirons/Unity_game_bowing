using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("旋转")]
    public float controlTime;
    public float rotation;
    public GameObject gunShoot;
    public float controlShoot;
    public TextMeshProUGUI SkillShoot;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        startRotate();
        judgeShoot();
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    void judgeShoot()
    {
        float t = Input.GetAxisRaw("Shoot");
        
        if(t != 0 && controlShoot == 0)
        {
            controlShoot = 0.01f;
            Vector3 a = new Vector3(transform.position.x + 0.01f, transform.position.y + 0.01f, 0);
            Instantiate(gunShoot, a, Quaternion.Euler(0, 0, rotation));
        }
        else if(controlShoot != 0)
        {
            controlShoot += Time.deltaTime;
            float temp = Mathf.Ceil(10f - controlShoot);
            SkillShoot.text = "射击：" + temp.ToString();
            if(controlShoot >= 10f)
            {
                controlShoot = 0;
                SkillShoot.text = "射击：可用";
            }
        }
    }

    void startRotate()
    {
        float a = Input.GetAxis("Rotate");
        controlTime += Time.deltaTime;
        if (controlTime > 0.2f)
        {
            float b = 0;
            if (a > 0) b = -0.2f;
            else if (a < 0) b = 0.2f;
            if (rotation + b > 80 || rotation + b < -80) return;
            rotation += b;
        }
    }

}
