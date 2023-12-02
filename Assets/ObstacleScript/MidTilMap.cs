using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MidTilMap : MonoBehaviour
{
    public RawImage rawImage;
    public float controlClliderTime;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && controlClliderTime == 0)
        {
            
            if (rawImage.rectTransform.sizeDelta.x > 0)
                rawImage.rectTransform.sizeDelta = new Vector2(rawImage.rectTransform.sizeDelta.x - 60, rawImage.rectTransform.sizeDelta.y);
            controlClliderTime = 0.01f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if(transform.position.y + 20 < player.position.y)
        //{
        //    Destroy(gameObject);
        //}
        if(controlClliderTime != 0)
        {
            controlClliderTime += Time.deltaTime;
            if (controlClliderTime > 2) controlClliderTime = 0;
        }
    }
}
