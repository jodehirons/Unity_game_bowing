using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public GameObject portal;
    public Rigidbody2D MyBody;
    public Transform gunTran;

    void Start()
    {
        MyBody= GetComponent<Rigidbody2D>();
        if(transform.position.y > -7)
        {
            float rotation = gunTran.rotation.z * 100;
            MyBody.velocity = new Vector2(Mathf.Cos((90 + rotation) * Mathf.Deg2Rad), Mathf.Sin((90 + rotation) * Mathf.Deg2Rad)) * 8;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Blood")
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "GunShoot" || collision.gameObject.tag == "Obstacle")
        {
            Vector3 a = new Vector3(transform.position.x, transform.position.y, 0);
            Instantiate(portal, a, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
}
